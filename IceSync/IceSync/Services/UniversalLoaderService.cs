using FluentResults;
using IceSync.Interfaces;
using IceSync.Common;
using IceSync.Models;
using IceSync.Models.Requests;
using IceSync.Models.Responses;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace IceSync.Services
{
    public sealed class UniversalLoaderService(IUniversalLoaderExternalApi externalApi, IOptions<UniversalLoaderApiSettings> options, IDistributedCache cache) : IUniversalLoaderService
    {
        public async Task<Result<List<Workflow>>> GetWorkflowsAsync()
        {
            var tokenResult = await GetTokenAsync();

            if (tokenResult.IsFailed)
                return Result.Fail<List<Workflow>>(tokenResult.Errors.First().Message);

            var response = await externalApi.GetWorkflowsAsync(tokenResult.Value);

            if (!response.IsSuccessStatusCode)
                return Result.Fail(response.Error?.Message ?? "Unknown error");

            return Result.Ok(response.Content!);
        }

        public async Task<Result<bool>> RunWorkflowAsync(int workflowId)
        {
            var tokenResult = await GetTokenAsync();

            if (tokenResult.IsFailed)
                return Result.Fail<bool>(tokenResult.Errors.First().Message);

            var response = await externalApi.RunWorkflowAsync(workflowId, tokenResult.Value);

            if (!response.IsSuccessStatusCode)
                return Result.Fail<bool>(response.Error.Content);

            return Result.Ok();
        }

        private readonly AuthenticationRequest _authRequest = new()
        {
            ApiCompanyId = options.Value.ApiCompanyId,
            ApiUserId = options.Value.ApiUserId,
            ApiUserSecret = options.Value.ApiUserSecret
        };

        private AuthenticationRequest GetAuthRequest() => _authRequest;

        private async Task<Result<string>> GetTokenAsync()
        {
            var cachedJson = await cache.GetStringAsync(Constants.TokenCacheKey);

            if (!string.IsNullOrEmpty(cachedJson))
            {
                var cached = JsonSerializer.Deserialize<AuthenticationResponse>(cachedJson);

                if (cached != null && cached.ExpiresOn > DateTimeOffset.UtcNow)
                    return Result.Ok($"{cached.TokenType} {cached.Token}");
            }

            var request = GetAuthRequest();
            var response = await externalApi.AuthenticateAsync(request);

            if (!response.IsSuccessStatusCode || response.Content == null)
                return Result.Fail(response.Error!.Message);

            var timeToExpireToken = response.Content.ExpiresIn - 30;
            var expiresOn = DateTimeOffset.UtcNow.AddSeconds(timeToExpireToken);

            var newToken = new AuthenticationResponse
            {
                TokenType = response.Content.TokenType,
                Token = response.Content.Token,
                ExpiresIn = response.Content.ExpiresIn,
                ExpiresOn = expiresOn
            };

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = expiresOn
            };

            var json = JsonSerializer.Serialize(newToken);
            await cache.SetStringAsync(Constants.TokenCacheKey, json, options);

            return Result.Ok($"{response.Content.TokenType} {response.Content.Token}");
        }
    }
}
