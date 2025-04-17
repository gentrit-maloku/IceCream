using IceSync.Enums;

namespace IceSync.Middlewares.Custom_Exceptions
{
    [Serializable]
    public sealed class ItemNotFoundException(string message) : Exception(message)
    {
        public ErrorCode Code => ErrorCode.ItemNotFound;
    }
}
