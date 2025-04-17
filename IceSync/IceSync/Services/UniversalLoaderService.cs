using FluentResults;
using IceSync.Common;
using IceSync.Interfaces;
using IceSync.Models;
using IceSync.Models.Requests;
using Microsoft.Extensions.Options;

namespace IceSync.Services
{
    public sealed class UniversalLoaderService(IUniversalLoaderExternalApi externalApi, IOptions<UniversalLoaderApiSettings> options) : IUniversalLoaderService
    {
        public async Task<Result<List<Workflow>>> GetWorkflowsAsync()
        {
            var authRequest = GetAuthRequest();
            var authResponse = await externalApi.AuthenticateAsync(authRequest);

            string token = $"{authResponse.Content!.TokenType} {authResponse.Content.Token}";

            var response = await externalApi.GetWorkflowsAsync(token);

            return Result.Ok(response.Content!);
        }

        private readonly AuthenticationRequest _authRequest = new()
        {
            ApiCompanyId = options.Value.ApiCompanyId,
            ApiUserId = options.Value.ApiUserId,
            ApiUserSecret = options.Value.ApiUserSecret
        };

        public AuthenticationRequest GetAuthRequest() => _authRequest;
    }
}
