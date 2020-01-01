using System.Collections.ObjectModel;

namespace ESO_Lang_Editor.View
{
    public class SearchTextInPosition : ObservableCollection<string>
    {
        public SearchTextInPosition() : base()
        {
            Add("包含全文");
            Add("仅包含开头");
            Add("仅包含结尾");
        }
    }

}
    
