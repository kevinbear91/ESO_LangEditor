using System.Collections.ObjectModel;

namespace ESO_Lang_Editor.View
{
    public class SearchType : ObservableCollection<string>
    {
        public SearchType() : base()
        {
            Add("搜编号");
            Add("搜英文");
            Add("搜译文");
        }
    }
}
