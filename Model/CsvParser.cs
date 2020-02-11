using ESO_Lang_Editor.View;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Convert;

namespace ESO_Lang_Editor.Model
{
    class CsvParser
    {
        /// <summary>
        /// 从传进的CSV文本，按照ID和Index从小到大排序。
        /// </summary>
        /// <param name="csvContent"></param>
        /// <returns></returns>
        public List<FileModel_Csv> CsvSort(List<FileModel_Csv> csvContent)
        {
            List<FileModel_Csv> OrderdList = csvContent.OrderBy(t => t.stringID).
                ThenBy(t => t.stringIndex).ToList();

            return OrderdList;
        }

        public Dictionary<string, string> CsvListToDict(List<FileModel_Csv> csvContent)
        {
            var Dict = new Dictionary<string, string>();

            foreach (var data in csvContent)
            {
                try
                {
                    Dict.Add(data.GetUniqueID(false), data.textContent);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Console.WriteLine(data.GetUniqueID(false), data.textContent);

                }
            }
            return Dict;
        }

        public List<LangSearchModel> CsvDictToModel(Dictionary<string, string> Dict)
        {
            var _langData = new List<LangSearchModel>();

            foreach (var data in Dict)
            {
                var keyField = data.Key.Split(new char[] { '-' }, 3);
                _langData.Add(new LangSearchModel
                {
                    ID_Type = ToInt32(keyField[0]),
                    ID_Unknown = ToInt32(keyField[1]),
                    ID_Index = ToInt32(keyField[2]),
                    Text_EN = data.Value
                });
            }
            return _langData;
        }


        public Dictionary<string, string> LoadCsvToDict(string path)
        {
            var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
            var reader = engine.ReadFile(path).ToList();
            var Dict = CsvListToDict(reader);

            return Dict;
        }

        public List<FileModel_Csv> LoadCsvToList(string path)
        {
            var engine = new FileHelperEngine<FileModel_Csv>(Encoding.UTF8);
            var reader = engine.ReadFile(path).ToList();

            return reader;
        }


        public Dictionary<string, string> CsvCompareNonChange(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        {
            var ContentnNonChange = new Dictionary<string, string>();

            foreach (var entry in DiffDictionary(OldDict, NewDict))
            {
                if (entry.Value == "same")
                    ContentnNonChange.Add(entry.Key, OldDict[entry.Key]);

            }
            return ContentnNonChange;
        }

        public Dictionary<string, string> CsvCompareEdited(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        {
            var ContentEdited = new Dictionary<string, string>();

            foreach (var entry in DiffDictionary(OldDict, NewDict))
            {
                if (entry.Value == "different")
                    ContentEdited.Add(entry.Key, NewDict[entry.Key]);

            }
            return ContentEdited;
        }

        public Dictionary<string, string> CsvCompareAdded(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        {
            var ContentAdded = new Dictionary<string, string>();

            foreach (var entry in DiffDictionary(OldDict, NewDict))
            {
                if (entry.Value == "added")
                    ContentAdded.Add(entry.Key, NewDict[entry.Key]);

            }
            return ContentAdded;
        }

        public Dictionary<string, string> CsvCompareRemove(Dictionary<string, string> OldDict, Dictionary<string, string> NewDict)
        {
            var ContentRemoved = new Dictionary<string, string>();

            foreach (var entry in DiffDictionary(OldDict, NewDict))
            {
                if (entry.Value == "removed")
                    ContentRemoved.Add(entry.Key, OldDict[entry.Key]);
            }
            return ContentRemoved;
        }


        private static Dictionary<string, string> DiffDictionary(Dictionary<string, string> first, Dictionary<string, string> second)
        {
            var diff = first.ToDictionary(e => e.Key, e => "removed");
            foreach (var other in second)
            {
                string firstValue;
                if (first.TryGetValue(other.Key, out firstValue))
                {
                    diff[other.Key] = firstValue.Equals(other.Value) ? "same" : "different";
                }
                else
                {
                    diff[other.Key] = "added";
                }
            }
            return diff;
        }

    }
}
