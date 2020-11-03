using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Convert;

namespace ESO_LangEditorLib.Models
{
    public class GameVersion
    {
        private Dictionary<string, string> gameVersion;

        public GameVersion()
        {
            InitGameVersionName();
        }

        public string GetVersionName(string fileid)
        {
            if (gameVersion.ContainsKey(fileid))
            {
                string category = gameVersion[fileid];
                return category;
            }
            return fileid.ToString();
        }

        private async void InitGameVersionName()
        {
            gameVersion = new Dictionary<string, string>();

            //fileidToCategory.Add("UI", "UI");

            string result;

            using (StreamReader reader = new StreamReader(@"Data\VersionInfo.txt"))
            {
                //Debug.WriteLine("Opened file.");

                string version;
                string text;

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { '=' }, 2);

                    version = words[0];
                    text = words[1];

                    gameVersion.Add(version, text);
                }
                reader.Close();
                //Debug.WriteLine("Total lines: " + fileidToCategory.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
        }
    }
}
