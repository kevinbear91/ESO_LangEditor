using Core.EnumTypes;
using Prism.Events;

namespace GUI.EventAggres
{
    public class ConnectStatusChangeEvent : PubSubEvent<ClientConnectStatus>
    {
    }
}
