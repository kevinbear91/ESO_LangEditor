using Core.Models;
using Prism.Events;
using System.Collections.Generic;

namespace GUI.EventAggres
{
    public class MainDataGridSelectedItemsToMainWindowVM : PubSubEvent<List<LangTextDto>>
    {

    }
}
