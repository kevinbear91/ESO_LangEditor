using ESO_LangEditor.Core.EnumTypes;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.EventAggres
{
    public class ConnectStatusChangeEvent : PubSubEvent<ClientConnectStatus>
    {
    }
}
