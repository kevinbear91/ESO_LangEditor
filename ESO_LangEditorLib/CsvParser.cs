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
    public class CsvParser
    {

        public async Task<List<LangData>> CsvReaderToListAsync(string path)
        #region 读取CSV文件并返回List<LangData>
        {
            string result;
            List<LangData> csvData = new List<LangData>();
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

            static void ParserCsvAddToList(List<LangData> csvData, out string id, out string unknown, out string index, out string text, string[] words)
            #region 分析CSV文件，并将分析后的文本加入 List<CsvData>
            {


                id = words[0].Trim('"');
                unknown = words[1].Trim('"');
                index = words[2].Trim('"');
                text = words[4].Substring(1, words[4].Length - 2);

                csvData.Add(new LangData
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


        public async Task<Dictionary<string, LangData>> CsvReaderToDictionaryAsync(string path)
        #region 读取CSV文件并返回Dictionary<string, LangData>
        {
            string result;
            Dictionary<string, LangData> csvData = new Dictionary<string, LangData>();
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
                        ParserCsvAddToList(csvData, out id, out unknown, out index, out text, words);
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
                            ParserCsvAddToList(csvData, out id, out unknown, out index, out text, words);
                        }
                    }
                }
                reader.Close();
                Debug.WriteLine("Total lines: " + csvData.Count);
                //MessageBox.Show("读取完毕，共 " + csvData.Count + " 行数据。");
            }
            return csvData;

            static void ParserCsvAddToList(Dictionary<string, LangData> csvData, out string id, out string unknown, out string index, out string text, string[] words)
            #region 分析CSV文件，并将分析后的文本加入 Dictionary<string, LangData>
            {


                id = words[0].Trim('"');
                unknown = words[1].Trim('"');
                index = words[2].Trim('"');
                text = words[4].Substring(1, words[4].Length - 2);

                string key = id + "-" + unknown + "-" + index;

                csvData.Add(key, new LangData
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
