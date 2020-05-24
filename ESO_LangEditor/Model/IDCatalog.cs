using System.Collections.Generic;

namespace ESO_Lang_Editor.Model
{
    class IDCatalog
    {
        private Dictionary<string, string> fileidToCategory;

        public IDCatalog()
        {
            InitFileidToCategory();
        }

        public string GetCategory(string fileid)
        {
            if (fileidToCategory.ContainsKey(fileid))
            {
                string category = fileidToCategory[fileid];
                return category;
            }
            return "";
        }

        private void InitFileidToCategory()
        {
            fileidToCategory = new Dictionary<string, string>();

            //fileidToCategory.Add("UI", "UI");
            
        }



    }

}
