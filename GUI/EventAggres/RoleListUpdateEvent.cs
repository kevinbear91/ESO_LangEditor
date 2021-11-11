using Prism.Events;
using System.Collections.Generic;

namespace GUI.EventAggres
{
    public class RoleListUpdateEvent : PubSubEvent<List<string>>
    {
    }
}
