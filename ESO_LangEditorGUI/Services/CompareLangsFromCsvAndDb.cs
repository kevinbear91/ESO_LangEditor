using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ESO_LangEditorGUI.Services
{
    public class CompareLangsFromCsvAndDb
    {
        private List<LangTextDto> _added { get; set; }

        private List<LangTextDto> _changed { get; set; }

        private List<LangTextDto> _nonChanged { get; set; }

        private List<LangTextDto> _removedList { get; set; }

        private Dictionary<string, LangTextDto> _removedDict { get; set; }

        private CompareWindowViewModel compareWindowViewModel { get; }

        private Dictionary<string, LangTextDto> _first { get; set; }
        private Dictionary<string, LangTextDto> _second { get; set; }


        public CompareLangsFromCsvAndDb(Dictionary<string, LangTextDto> first, Dictionary<string, LangTextDto> second)
        {
            
        }

        public void CompareDicts()
        {
            Debug.WriteLine("开始比较。");

            _removedDict = _first;

            foreach (var other in _second)
            {

                if (_first.TryGetValue(other.Key, out LangTextDto firstValue))
                {
                    if (firstValue.TextEn.Equals(other.Value.TextEn))
                    {
                        _nonChanged.Add(firstValue);
                        _removedDict.Remove(other.Key);
                    }
                    else
                    {
                        _changed.Add(new LangTextDto
                        {
                            TextId = other.Value.TextId,
                            //ID = other.Value.ID,
                            //Unknown = other.Value.Unknown,
                            //Lang_Index = other.Value.Lang_Index,
                            TextEn = other.Value.TextEn,
                            TextZh = firstValue.TextZh,
                            //UpdateStats = VersionInput_textBox.Text,
                            IsTranslated = firstValue.IsTranslated,
                            //review = 2,
                        });
                        _removedDict.Remove(other.Key);
                    }
                }
                else
                {
                    _added.Add(new LangTextDto
                    {
                        TextId = other.Value.TextId,
                        //ID = other.Value.ID,
                        //Unknown = other.Value.Unknown,
                        //Lang_Index = other.Value.Lang_Index,
                        TextEn = other.Value.TextEn,
                        TextZh = other.Value.TextEn,
                        //UpdateStats = VersionInput_textBox.Text,
                        IsTranslated = 0,
                        //RowStats = 1,
                    });
                    _removedDict.Remove(other.Key);
                }
            }

            _removedList = _removedDict.Values.ToList();
        }
    }
}
