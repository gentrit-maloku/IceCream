namespace IceSync.Models.Requests
{
    public sealed record AuthenticationRequest
    {
        public string ApiCompanyId { get; init; }

        public string ApiUserId { get; init; }

        public string ApiUserSecret { get; init; }
    }
}
