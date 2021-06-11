using ESO_LangEditor.Core.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class MainDataGridSelectedItemsToMainWindowVM : PubSubEvent<List<LangTextDto>>
    {

    }
}
