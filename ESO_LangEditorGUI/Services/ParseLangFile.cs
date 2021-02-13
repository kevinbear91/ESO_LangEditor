using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorGUI.Services
{
    public class ParseLangFile
    {


        public async Task<Dictionary<string, LangTextDto>> CsvParserToDictionaryAsync(string path)
        #region 读取CSV文件并返回Dictionary<string, LangData>
        {
            string result;
            Dictionary<string, LangTextDto> csvData = new Dictionary<string, LangTextDto>();
            using (StreamReader reader = new StreamReader(path))
            {
                Debug.WriteLine("Opened file.");

                string id;
                string unknown;
                string index;
                string text;
                bool passedFirstLine = false;

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { ',' }, 5);

                    if (passedFirstLine)
                    {
                        ParserCsvAddToDict(csvData, out id, out unknown, out index, out text, words);
                    }
                    else
                    {
                        id = words[0].Trim('"');

                        if (id == "ID")
                        {
                            passedFirstLine = true;
                            Debug.WriteLine("Have header line, set to skip.");
                        }
                        else
                        {
                            ParserCsvAddToDict(csvData, out id, out unknown, out index, out text, words);
                        }
                    }
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static void ParserCsvAddToDict(Dictionary<string, LangTextDto> csvData, out string id, out string unknown, out string index, out string text, string[] words)
            #region 分析CSV文件，并将分析后的文本加入 Dictionary<string, LangData>
            {


                id = words[0].Trim('"');
                unknown = words[1].Trim('"');
                index = words[2].Trim('"');
                text = words[4].Substring(1, words[4].Length - 2).Replace("\"\"", "\"");
                string key = id + "-" + unknown + "-" + index;

                csvData.Add(key, new LangTextDto
                {
                    IdType = ToInt32(id),
                    TextId = key,
                    TextEn = text,
                    LangTextType = LangType.LangText,


                    //UniqueID = key,
                    //ID = ToInt32(id),
                    //Unknown = ToInt32(unknown),
                    //Lang_Index = ToInt32(index),
                    //Text_EN = text,
                });

                //Debug.WriteLine("ID: " + id + ", "
                //    + "Unknown: " + unknown + ", "
                //    + "Index: " + index + ", "
                //    + "Text: " + text);
            }
            #endregion
        }
        #endregion


        public async Task<Dictionary<string, LangTextDto>> LuaParser(List<string> luaPaths)
        {
            bool isClient;
            string input;
            string pattern = @"^[\s]*SafeAddString\(?[\s]*(\w+)[,\s]+\""(.+)\""[^\""]+$";

            var luaResult = new Dictionary<string, LangTextDto>();

            foreach (var file in luaPaths)
            {
                if (file.EndsWith("en_client.lua"))
                    isClient = true;
                else
                    isClient = false;

                using StreamReader sr = new StreamReader(file);
                while ((input = await sr.ReadLineAsync()) != null)
                {
                    foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                    {
                        string id = match.Groups[1].Value;
                        string text_en = match.Groups[2].Value;
                        int idType = 100;

                        if (luaResult.Count >= 1 && luaResult.TryGetValue(id, out LangTextDto luaResultValue))
                        {
                            if (luaResultValue.TextId.Contains(id))
                            {

                                luaResult[id].LangTextType = LangType.LuaBoth;
                            }
                            else
                            {
                                if (isClient)
                                {
                                    luaResult.Add(id, new LangTextDto
                                    {
                                        TextId = id,
                                        IdType = idType,
                                        TextEn = text_en,
                                        LangTextType = LangType.LuaClient,
                                    });
                                }
                                else
                                {
                                    luaResult.Add(id, new LangTextDto
                                    {
                                        TextId = id,
                                        IdType = idType,
                                        TextEn = text_en,
                                        LangTextType = LangType.LuaPreGame,
                                    });
                                }
                            }

                        }
                        else
                        {
                            if (isClient)
                            {
                                luaResult.Add(id, new LangTextDto
                                {
                                    TextId = id,
                                    IdType = idType,
                                    TextEn = text_en,
                                    LangTextType = LangType.LuaClient,
                                });
                            }
                            else
                            {
                                luaResult.Add(id, new LangTextDto
                                {
                                    TextId = id,
                                    IdType = idType,
                                    TextEn = text_en,
                                    LangTextType = LangType.LuaPreGame,
                                });
                            }
                        }




                        //Debug.WriteLine("ID: {0}, Content: {1}",
                        //                  match.Groups[1].Value, match.Groups[2].Value);
                    }

                }
            }

            //foreach(var lua in luaResult)
            //{
            //    Debug.WriteLine("ID: {0}, EN: {1}, dataenum: {2}", lua.Value.UniqueID, lua.Value.Text_EN, lua.Value.DataEnum);
            //}

            //Debug.WriteLine(luaResult.Count);
            return luaResult;
        }

        public List<LangTextDto> JsonToLangTextListReader(string path)
        {

            JsonFileDto jsonFile;// = new JsonDto();
            string jsonString;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

            };

            using (StreamReader reader = new StreamReader(path))
            {
                jsonString = reader.ReadToEnd();
            }

            jsonFile = JsonSerializer.Deserialize<JsonFileDto>(jsonString);

            //foreach (var zh in jsonFile.LangTexts)
            //{
            //    Debug.Write(zh.TextZh);
            //}

            return jsonFile.LangTexts;

            //string jsonString = JsonSerializer.Deserialize(json, options);
        }

        public JsonFileDto JsonToDtoReader(string path)
        {

            //JsonDto jsonFile;// = new JsonDto();
            string jsonString;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),

            };

            using (StreamReader reader = new StreamReader(path))
            {
                jsonString = reader.ReadToEnd();
            }

            return JsonSerializer.Deserialize<JsonFileDto>(jsonString);
        }

        public List<LangTextDto> LangTextReaderToListAsync(string path)
        #region 读取 .LangDB 文件并返回List<LangData>
        {
            string result;
            List<LangTextDto> fileContent = new List<LangTextDto>();
            using (StreamReader reader = new StreamReader(path))
            {
                Debug.WriteLine("Opened file.");

                string uniqueID;
                string id;
                string unknown;
                string index;
                string text;
                string en;
                string zh;
                string translate;
                string updateStats;
                string rowStats;

                //bool passedFirstLine = false;

                while ((result = reader.ReadLine()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { '\v' }, 9);

                    if (words[0].StartsWith("SI_"))
                    {
                        uniqueID = words[0];
                        en = words[1];
                        zh = words[2];
                        translate = words[3];
                        rowStats = words[4];
                        fileContent.Add(new LangTextDto
                        {
                            //Id = new Guid(),
                            TextId = uniqueID,
                            IdType = 100,
                            TextEn = en,
                            TextZh = zh,
                            //LangType = LangType.
                            IsTranslated = ToByte(translate),
                            UpdateStats = "Update27",
                            //RowStats = ToInt32(rowStats),
                        });
                    }
                    else
                    {
                        uniqueID = words[0];
                        id = words[1];
                        //unknown = words[2];
                        //index = words[3];
                        en = words[4];
                        zh = words[5];
                        updateStats = words[6];
                        translate = words[7];
                        rowStats = words[8];

                        fileContent.Add(new LangTextDto
                        {
                            TextId = uniqueID,
                            IdType = ToInt32(id),
                            TextEn = en,
                            TextZh = zh,
                            UpdateStats = updateStats,
                            //UniqueID = uniqueID,
                            //ID = ToInt32(id),
                            //Unknown = ToInt32(unknown),
                            //Lang_Index = ToInt32(index),
                            //Text_EN = text,
                            //Text_ZH = zh,
                            //IsTranslated = ToInt32(translate),
                            //RowStats = ToInt32(rowStats),
                        });
                    }

                }
                reader.Close();
                Debug.WriteLine("Total lines: " + fileContent.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return fileContent;

            //static void ParserLangTextAddToList(List<LangTextDto> csvData, out string uniqueID, out string id, out string unknown, out string index, out string text, out string zh, out string translate, out string rowStats, string[] words)
            //#region 分析.LangDB 文件，并将分析后的文本加入 List<CsvData>
            //{

            //    uniqueID = words[0];
            //    id = words[1];
            //    unknown = words[2];
            //    index = words[3];
            //    text = words[4];
            //    zh = words[5];
            //    translate = words[7];
            //    rowStats = words[8];

            //    csvData.Add(new LangTextDto
            //    {
            //        //UniqueID = uniqueID,
            //        //ID = ToInt32(id),
            //        //Unknown = ToInt32(unknown),
            //        //Lang_Index = ToInt32(index),
            //        //Text_EN = text,
            //        //Text_ZH = zh,
            //        //IsTranslated = ToInt32(translate),
            //        //RowStats = ToInt32(rowStats),
            //    }); ;

            //    Debug.WriteLine("ID: " + id + ", "
            //        + "Unknown: " + unknown + ", "
            //        + "Index: " + index + ", "
            //        + "Text: " + text + ", "
            //        + "zh: " + zh + ", "
            //        + "translate: " + translate + ", "
            //        + "rowStats: " + rowStats);
            //}
            //#endregion

            //static void ParserLangUIAddToList(List<LuaUIData> csvData, out string uniqueID, out string en, out string zh, out string translate, out string rowStats, string[] words)
            //#region 分析.LangUI 文件，并将分析后的文本加入 List<CsvData>
            //{

            //    uniqueID = words[0];
            //    en = words[1];
            //    zh = words[2];
            //    translate = words[3];
            //    rowStats = words[4];

            //    csvData.Add(new LuaUIData
            //    {
            //        UniqueID = uniqueID,
            //        Text_EN = en,
            //        Text_ZH = zh,
            //        IsTranslated = ToInt32(translate),
            //        RowStats = ToInt32(rowStats),
            //    }); ;

            //    Debug.WriteLine("ID: " + uniqueID + ", "
            //        + "zh: " + zh + ", "
            //        + "translate: " + translate + ", "
            //        + "rowStats: " + rowStats);
            //}
            //#endregion
        }
        #endregion

    }
}
