using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ESO_LangEditorGUI.Services
{
    public class LangExportFromDb
    {
        public LangExportFromDb()
        {

        }

        public void ExportText(IList<string> Id, IList<string> content)
        {
            var outputIDFile = Id;
            var outputTextFile = content;

            //var db = new LangDbController();

            //var dbData = await db.GetAllLangsListAsync();

            if (!Directory.Exists("_tmp"))
                Directory.CreateDirectory("_tmp");

            //foreach (var d in dbData)
            //{
            //    outputIDFile.Add(d.UniqueID);
            //    outputTextFile.Add(d.Text_ZH);
            //}

            using (StreamWriter sw = new StreamWriter("_tmp/ID.txt"))
            {
                foreach (string s in outputIDFile)
                {
                    sw.WriteLine(s);
                }
                sw.Flush();
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter("_tmp/Text.txt"))
            {
                foreach (string s in outputTextFile)
                {
                    sw.WriteLine(s);
                }
                sw.Flush();
                sw.Close();
            }
        }

        public void ExportLua(List<string> luaClientData, List<string> luaPregameData)
        {
            List<string> clientData = new List<string>();
            List<string> pregameData = new List<string>();
            //List<string> fontLib = new List<string>();
            string line;

            //var db = new LangDbController();


            //var data = await db.GetAllLuaLangsListAsync();

            StreamReader file = new StreamReader(@"Data\FontLib.txt");

            while ((line = file.ReadLine()) != null)
            {
                clientData.Add(line);
                pregameData.Add(line);
            }
            file.Close();

            clientData.AddRange(luaClientData);
            pregameData.AddRange(luaPregameData);

            //foreach (var d in data)
            //{
            //    //if(d.DataEnum == 3)

            //    switch (d.DataEnum)
            //    {
            //        case 1:
            //            luaPregameData.Add("[" + d.UniqueID + "]"
            //            + " = "
            //            + "\"" + d.Text_ZH + "\"");
            //            break;
            //        case 2:
            //            luaClientData.Add("[" + d.UniqueID + "]"
            //            + " = "
            //            + "\"" + d.Text_ZH + "\"");
            //            break;
            //        case 3:
            //            luaPregameData.Add("[" + d.UniqueID + "]"
            //            + " = "
            //            + "\"" + d.Text_ZH + "\"");
            //            luaClientData.Add("[" + d.UniqueID + "]"
            //            + " = "
            //            + "\"" + d.Text_ZH + "\"");
            //            break;
            //    }

            //}

            if (!Directory.Exists("Export"))
                Directory.CreateDirectory("Export");

            using (StreamWriter sw = new StreamWriter(@"Export\zh_client.str"))
            {

                foreach (string s in clientData)
                {
                    sw.WriteLine(s);
                }

                sw.Flush();
                sw.Close();
            }
            using (StreamWriter sw = new StreamWriter(@"Export\zh_pregame.str"))
            {

                foreach (string s in pregameData)
                {
                    sw.WriteLine(s);
                }

                sw.Flush();
                sw.Close();
            }
        }
        public async Task ExportJsonAsync(List<LangTextDto> translatedLangTexts)
        {
            string jsonString;
            //jsonString = JsonSerializer.Serialize(JsonDto);

            var json = new JsonFileDto();

            json.LangTexts = translatedLangTexts;
            json.ExportTime = DateTime.Now;

            jsonString = JsonSerializer.Serialize(json);

            string filName = GetTimeToFileName();

            using (FileStream fs = File.Create(@"Export\Translate_" + filName + ".json"))
            {
                await JsonSerializer.SerializeAsync(fs, jsonString);
            }

        }

        public string ExportTranslatedLang(List<LangTextDto> translatedLangTexts)
        {
            string filName = GetTimeToFileName();
            //List<LangTextDto> data = translatedLangTexts;

            if (!Directory.Exists("Export"))
                Directory.CreateDirectory("Export");

            string dbPath = @"Export\Translate_" + filName + ".LangDB";

            if (File.Exists(dbPath))
            {
                ExportTranslatedLang(translatedLangTexts);
            }
            else
            {
                ExportLangListFullColumnAsText(translatedLangTexts, "Export", "Translate_" + filName + ".LangDB");
            }

            return dbPath;
        }

        public void ExportLangListFullColumnAsText(List<LangTextDto> data, string directory, string fileName)
        {
            var outputText = new List<string>();


            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            foreach (var d in data)
            {
                //能找到个合适的分隔符真鸡巴难，\v = 匹配垂直制表符，\u000B
                //备用替代"`"
                int translate = 2;
                //outputText.Add(d.UniqueID
                //    + "\v" + d.ID
                //    + "\v" + d.Unknown
                //    + "\v" + d.Lang_Index
                //    + "\v" + d.Text_EN
                //    + "\v" + d.Text_ZH
                //    + "\v" + d.UpdateStats
                //    + "\v" + translate
                //    + "\v" + d.RowStats);
            }

            using (StreamWriter sw = new StreamWriter(directory + "/" + fileName))
            {
                foreach (string s in outputText)
                {
                    sw.WriteLine(s);
                }
                sw.Flush();
                sw.Close();
            }

        }


        private string GetTimeToFileName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }
}
