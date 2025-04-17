namespace IceSync.Common
{
    public sealed record UniversalLoaderApiSettings
    {
        public string ApiCompanyId { get; init; }

        public string ApiUserId { get; init; }

        public string ApiUserSecret { get; init; }

        public string BaseUrl { get; set; }
    }
}
