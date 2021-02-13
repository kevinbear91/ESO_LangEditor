using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditor.Core.Models
{
    public class IDCatalog
    {
        private Dictionary<int, string> fileidToCategory;

        public IDCatalog()
        {
             InitFileidToCategory();
        }

        public string GetCategory(int fileid)
        {
            if (fileidToCategory.ContainsKey(fileid))
            {
                string category = fileidToCategory[fileid];
                return category;
            }
            return fileid.ToString();
        }

        private async Task InitFileidToCategory()
        {
            fileidToCategory = new Dictionary<int, string>();

            //fileidToCategory.Add("UI", "UI");

            string result;

            using (StreamReader reader = new StreamReader(@"Data\IDCatalog.txt"))
            {
                //Debug.WriteLine("Opened file.");

                int id;
                string text;

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { '=' }, 2);

                    id = ToInt32(words[0]);
                    text = words[1];

                    fileidToCategory.Add(id, text);
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + fileidToCategory.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
        }



    }

}
