using IceSync.Enums;

namespace IceSync.Middlewares.Custom_Exceptions
{
    [Serializable]
    public sealed class AccessForbiddenException(string message) : Exception(message)
    {
        public ErrorCode Code => ErrorCode.AccessForbidden;
    }
}
