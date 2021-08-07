using ESO_LangEditor.Core.EnumTypes;
using Prism.Events;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class ConnectStatusChangeEvent : PubSubEvent<ClientConnectStatus>
    {
    }
}
