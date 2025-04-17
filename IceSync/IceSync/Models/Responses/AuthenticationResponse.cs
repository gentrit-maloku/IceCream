using System.Text.Json.Serialization;

namespace IceSync.Models.Responses
{
    public sealed record AuthenticationResponse
    {
        [JsonPropertyName("access_token")]
        public string Token { get; init; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        public DateTimeOffset ExpiresOn { get; set; }
    }
}
