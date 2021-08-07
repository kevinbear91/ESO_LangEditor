using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ESO_LangEditor.GUI.Services
{
    public class ExportDbToFile
    {

        public string ExportLangTextsAsJson(List<LangTextDto> langtextsList, LangChangeType changeType)
        {
            //var langtexts = langtextsList;

            //string jsonString;
            //jsonString = JsonSerializer.Serialize(JsonDto);

            string fileName;

            langtextsList.ForEach(lang => lang.IsTranslated = 2);

            var json = new JsonFileDto
            {
                LangTexts = langtextsList,
                Version = "1",
                ExportTime = DateTime.Now,
                ChangeType = changeType,
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

            };

            var options1 = new JsonWriterOptions
            {
                Indented = true
            };

            //jsonString = JsonSerializer.Serialize(json);
            //byte[] jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json, options);

            //byte[] utf8Json = JsonSerializer.SerializeToUtf8Bytes(json, options);
            string jsonString = JsonSerializer.Serialize(json, options);

            if (changeType == LangChangeType.ChangedZH)
            {
                fileName = @"Export\Translate_" + GetTimeToFileName() + ".json";
            }
            else
            {
                fileName = @"Export\DatabaseRev_" + GetTimeToFileName() + "_" + changeType.ToString() + ".json";
            }


            try
            {

                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(jsonString);
                    //await JsonSerializer.SerializeAsync(fs, jsonString);
                }

                return fileName;
            }
            catch (DirectoryNotFoundException)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.WriteLine(jsonString);
                    //await JsonSerializer.SerializeAsync(fs, jsonString);
                }
                return fileName;
            }

        }

        public void ExportText(List<LangTextDto> langList)
        {
            var outputIDFile = new List<string>();
            var outputTextFile = new List<string>();

            //var db = new LangDbController();

            //var dbData = await db.GetAllLangsListAsync();

            if (!Directory.Exists("_tmp"))
                Directory.CreateDirectory("_tmp");

            foreach (var l in langList)
            {
                outputIDFile.Add(l.TextId);
                outputTextFile.Add(l.TextZh);
            }

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

        public void ExportLua(List<LangTextDto> langList)
        {
            List<string> clientData = new List<string>();
            List<string> pregameData = new List<string>();
            string line;

            StreamReader file = new StreamReader(@"Data\FontLib.txt");

            while ((line = file.ReadLine()) != null)
            {
                clientData.Add(line);
                pregameData.Add(line);
            }
            file.Close();

            //clientData.AddRange(luaClientData);
            //pregameData.AddRange(luaPregameData);

            foreach (var d in langList)
            {
                switch (d.LangTextType)
                {
                    case LangType.LuaPreGame:
                        pregameData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                    case LangType.LuaClient:
                        clientData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                    case LangType.LuaBoth:
                        pregameData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        clientData.Add("[" + d.TextId + "]"
                        + " = "
                        + "\"" + d.TextZh + "\"");
                        break;
                }

            }

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

        private string GetTimeToFileName()
        {
            return DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        }
    }
}
