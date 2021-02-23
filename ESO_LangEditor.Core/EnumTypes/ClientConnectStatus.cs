
namespace ESO_LangEditor.Core.EnumTypes
{
    public enum ClientConnectStatus : byte
    {
        Connecting = 0,
        Login,
        Logout,
        ConnectError,
        SyncData,
        Updating,
    }
}
