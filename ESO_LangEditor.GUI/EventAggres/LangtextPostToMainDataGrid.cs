using ESO_LangEditor.Core.Models;
using Prism.Events;
using System.Collections.Generic;

namespace ESO_LangEditor.GUI.EventAggres
{
    public class LangtextPostToMainDataGrid : PubSubEvent<List<LangTextDto>>
    {

    }
}
