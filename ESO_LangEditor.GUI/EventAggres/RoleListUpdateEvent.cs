using Prism.Events;
using System.Collections.Generic;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class RoleListUpdateEvent : PubSubEvent<List<string>>
    {
    }
}
