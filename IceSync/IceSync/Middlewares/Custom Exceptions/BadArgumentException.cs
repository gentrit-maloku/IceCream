using IceSync.Enums;

namespace IceSync.Middlewares.Custom_Exceptions
{
    [Serializable]
    public sealed class BadArgumentException(string message) : Exception(message)
    {
        public ErrorCode Code => ErrorCode.BadArgument;
    }
}
