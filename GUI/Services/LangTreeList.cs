using Core.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Services
{
    public class LangTreeCategory
    {
        public string Name { get; }
        public ObservableCollection<LangTreeList> LangTree { get; }

        public LangTreeCategory(string name, params LangTreeList[] langTreeLists)
        {
            Name = name;
            LangTree = new ObservableCollection<LangTreeList>(langTreeLists);
        }
    }

    public class LangTreeList
    {
        public string ListName { get; }
        public ObservableCollection<LangTreeContent> LangContent { get; }

        public LangTreeList(string name, params LangTreeContent[] langTreeLists)
        {
            ListName = name;
            LangContent = new ObservableCollection<LangTreeContent>(langTreeLists);
        }
    }

    public class LangTreeContent
    {
        public string TypeName { get; }
        public LangTypeCategory TypeCategory { get; }

        public LangTreeContent(string name, LangTypeCategory typeCategory)
        {
            TypeName = name;
            TypeCategory = typeCategory;
        }
    }
}
