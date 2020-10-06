using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;

namespace ESO_LangEditorLib
{
    public class ParserCsv
    {

        public async Task<List<LangTextDto>> CsvReaderToListAsync(string path)
        #region 读取CSV文件并返回List<LangData>
        {
            string result;
            List<LangTextDto> csvData = new List<LangTextDto>();
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
                    
                    if(passedFirstLine)
                    {
                        ParserCsvAddToList(csvData, out id, out unknown, out index, out text, words);
                    }
                    else
                    {
                        id = words[0].Trim('"');

                        if (id == "ID")
                        { 
                            passedFirstLine = true;
                            Debug.WriteLine("Have header line, set to skip." );
                        }
                        else
                        {
                            ParserCsvAddToList(csvData, out id, out unknown, out index, out text, words);
                        }
                    }
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static void ParserCsvAddToList(List<LangTextDto> csvData, out string id, out string unknown, out string index, out string text, string[] words)
            #region 分析CSV文件，并将分析后的文本加入 List<CsvData>
            {


                id = words[0].Trim('"');
                unknown = words[1].Trim('"');
                index = words[2].Trim('"');
                text = words[4].Substring(1, words[4].Length - 2);

                csvData.Add(new LangTextDto
                {
                    UniqueID = id + "-" + unknown + "-" + index,
                    ID = ToInt32(id),
                    Unknown = ToInt32(unknown),
                    Lang_Index = ToInt32(index),
                    Text_EN = text,
                }); ;

                //Debug.WriteLine("ID: " + id + ", "
                //    + "Unknown: " + unknown + ", "
                //    + "Index: " + index + ", "
                //    + "Text: " + text);
            }
            #endregion
        }
        #endregion

        public async Task<List<LangTextDto>> ExportReaderToListAsync(string path)
        #region 读取 .LangDB 文件并返回List<LangData>
        {
            string result;
            List<LangTextDto> csvData = new List<LangTextDto>();
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
                string rowStats;

                //bool passedFirstLine = false;

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { '\v' }, 9);
                    ParserCsvAddToList(csvData, out uniqueID, out id, out unknown, out index, out text, out zh, out translate, out rowStats, words);

                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static void ParserCsvAddToList(List<LangTextDto> csvData, out string uniqueID, out string id, out string unknown, out string index, out string text, out string zh, out string translate, out string rowStats,  string[] words)
            #region 分析.LangDB 文件，并将分析后的文本加入 List<CsvData>
            {

                uniqueID = words[0];
                id = words[1];
                unknown = words[2];
                index = words[3];
                text = words[4];
                zh = words[5];
                translate = words[7];
                rowStats = words[8];

                csvData.Add(new LangTextDto
                {
                    UniqueID = uniqueID,
                    ID = ToInt32(id),
                    Unknown = ToInt32(unknown),
                    Lang_Index = ToInt32(index),
                    Text_EN = text,
                    Text_ZH = zh,
                    IsTranslated = ToInt32(translate),
                    RowStats = ToInt32(rowStats),
                }); ;

                Debug.WriteLine("ID: " + id + ", "
                    + "Unknown: " + unknown + ", "
                    + "Index: " + index + ", "
                    + "Text: " + text + ", "
                    + "zh: " + zh + ", "
                    + "translate: " + translate + ", "
                    + "rowStats: " + rowStats);
            }
            #endregion
        }
        #endregion

        public async Task<List<LuaUIData>> ExportLuaReaderToListAsync(string path)
        #region 读取 .LangUI 文件并返回List<LuaUIData>
        {
            string result;
            List<LuaUIData> csvData = new List<LuaUIData>();
            using (StreamReader reader = new StreamReader(path))
            {
                Debug.WriteLine("Opened file.");

                string uniqueID;
                string en;
                string zh;
                string translate;
                string rowStats;

                //bool passedFirstLine = false;

                while ((result = await reader.ReadLineAsync()) != null)
                {
                    string[] words = result.Trim().Split(new char[] { '\v' }, 9);
                    ParserCsvAddToList(csvData, out uniqueID, out en, out zh, out translate, out rowStats, words);

                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static void ParserCsvAddToList(List<LuaUIData> csvData, out string uniqueID, out string en, out string zh, out string translate, out string rowStats, string[] words)
            #region 分析.LangUI 文件，并将分析后的文本加入 List<CsvData>
            {

                uniqueID = words[0];
                en = words[1];
                zh = words[2];
                translate = words[3];
                rowStats = words[4];

                csvData.Add(new LuaUIData
                {
                    UniqueID = uniqueID,
                    Text_EN = en,
                    Text_ZH = zh,
                    IsTranslated = ToInt32(translate),
                    RowStats = ToInt32(rowStats),
                }); ;

                Debug.WriteLine("ID: " + uniqueID + ", "
                    + "zh: " + zh + ", "
                    + "translate: " + translate + ", "
                    + "rowStats: " + rowStats);
            }
            #endregion
        }
        #endregion


        public async Task<Dictionary<string, LangTextDto>> CsvReaderToDictionaryAsync(string path)
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
                text = words[4].Substring(1, words[4].Length - 2).Replace("\"\"","\"");
                string key = id + "-" + unknown + "-" + index;

                csvData.Add(key, new LangTextDto
                {
                    UniqueID = key,
                    ID = ToInt32(id),
                    Unknown = ToInt32(unknown),
                    Lang_Index = ToInt32(index),
                    Text_EN = text,
                }); ;

                //Debug.WriteLine("ID: " + id + ", "
                //    + "Unknown: " + unknown + ", "
                //    + "Index: " + index + ", "
                //    + "Text: " + text);
            }
            #endregion
        }
        #endregion




        /// <summary>
        /// 从传进的CSV文本，按照ID和Index从小到大排序。
        /// </summary>
        /// <param name="csvContent"></param>
        /// <returns></returns>
        //public List<FileModel_Csv> CsvSort(List<FileModel_Csv> csvContent)
        //{
        //    List<FileModel_Csv> OrderdList = csvContent.OrderBy(t => t.stringID).
        //        ThenBy(t => t.stringIndex).ToList();

        //    return OrderdList;
        //}

        //public Dictionary<string, string> CsvListToDict(List<FileModel_Csv> csvContent)
        //{
        //    var Dict = new Dictionary<string, string>();

        //    foreach (var data in csvContent)
        //    {
        //        try
        //        {
        //            Dict.Add(data.GetUniqueID(false), data.textContent);
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.ToString());
        //            Console.WriteLine(data.GetUniqueID(false), data.textContent);

        //        }
        //    }
        //    return Dict;
        //}

        //public List<LangSearchModel> CsvDictToModel(Dictionary<string, string> Dict)
        //{
        //    var _langData = new List<LangSearchModel>();

        //    foreach (var data in Dict)
        //    {
        //        var keyField = data.Key.Split(new char[] { '-' }, 3);
        //        _langData.Add(new LangSearchModel
        //        {
        //            ID_Type = ToInt32(keyField[0]),
        //            ID_Unknown = ToInt32(keyField[1]),
        //            ID_Index = ToInt32(keyField[2]),
        //            Text_EN = data.Value
        //        });
        //    }
        //    return _langData;
        //}


        //public Dictionary<string, string> LoadCsvToDict(string path)
        //{
        //    var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
        //    var reader = engine.ReadFile(path).ToList();
        //    var Dict = CsvListToDict(reader);

        //    return Dict;
        //}

        //public List<FileModel_Csv> LoadCsvToList(string path)
        //{
        //    var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
        //    var reader = engine.ReadFile(path).ToList();

        //    return reader;
        //}


        //public Dictionary<string, string> CsvCompareNonChange(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        //{
        //    var ContentnNonChange = new Dictionary<string, string>();

        //    foreach (var entry in DiffDictionary(OldDict, NewDict))
        //    {
        //        if (entry.Value == "same")
        //            ContentnNonChange.Add(entry.Key, OldDict[entry.Key]);

        //    }
        //    return ContentnNonChange;
        //}

        //public Dictionary<string, string> CsvCompareEdited(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        //{
        //    var ContentEdited = new Dictionary<string, string>();

        //    foreach (var entry in DiffDictionary(OldDict, NewDict))
        //    {
        //        if (entry.Value == "different")
        //            ContentEdited.Add(entry.Key, NewDict[entry.Key]);

        //    }
        //    return ContentEdited;
        //}

        //public Dictionary<string, string> CsvCompareAdded(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        //{
        //    var ContentAdded = new Dictionary<string, string>();

        //    foreach (var entry in DiffDictionary(OldDict, NewDict))
        //    {
        //        if (entry.Value == "added")
        //            ContentAdded.Add(entry.Key, NewDict[entry.Key]);

        //    }
        //    return ContentAdded;
        //}

        //public Dictionary<string, string> CsvCompareRemove(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        //{
        //    var ContentRemoved = new Dictionary<string, string>();

        //    foreach (var entry in DiffDictionary(OldDict, NewDict))
        //    {
        //        if (entry.Value == "removed")
        //            ContentRemoved.Add(entry.Key, OldDict[entry.Key]);
        //    }
        //    return ContentRemoved;
        //}


        

    }
}
