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
        private List<LangTextDto> _added = new List<LangTextDto>(); 

        private List<LangTextDto> _changed = new List<LangTextDto>();

        private List<LangTextDto> _nonChanged = new List<LangTextDto>();

        private List<LangTextDto> _removedList = new List<LangTextDto>();

        private Dictionary<string, LangTextDto> _removedDict = new Dictionary<string, LangTextDto>();

        private CompareWindowViewModel _compareWindowViewModel { get; }

        private Dictionary<string, LangTextDto> _first;
        private Dictionary<string, LangTextDto> _second; 


        public CompareLangsFromCsvAndDb(CompareWindowViewModel compareWindowViewModel)
        {
            _compareWindowViewModel = compareWindowViewModel;

            _first = _compareWindowViewModel.DbDict;
            _second = _compareWindowViewModel.CsvDict;


            //CompareDicts();
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

            _compareWindowViewModel.Added = _added;
            _compareWindowViewModel.Changed = _changed;
            _compareWindowViewModel.RemovedList = _removedList;

            _compareWindowViewModel.AddedTag = _added.Count.ToString();
            _compareWindowViewModel.ChangedTag = _changed.Count.ToString();
            _compareWindowViewModel.RemovedTag = _removedList.Count.ToString();
        }
    }
}
