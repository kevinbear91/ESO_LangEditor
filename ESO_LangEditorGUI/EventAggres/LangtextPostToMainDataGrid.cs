using ESO_LangEditor.Core.Models;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.EventAggres
{
    public class LangtextPostToMainDataGrid : PubSubEvent<List<LangTextDto>>
    {

    }
}
