using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.ViewModels;
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
                            Id = firstValue.Id,
                            TextId = firstValue.TextId,
                            IdType = firstValue.IdType,
                            TextEn = other.Value.TextEn,
                            TextZh = firstValue.TextZh,
                            UpdateStats = _compareWindowViewModel.UpdateVersionText,
                            IsTranslated = firstValue.IsTranslated,
                            EnLastModifyTimestamp = DateTime.Now,
                            ZhLastModifyTimestamp = firstValue.ZhLastModifyTimestamp,
                            LangTextType = firstValue.LangTextType,
                            UserId = App.LangConfig.UserGuid,
                            //review = 2,
                        });
                        _removedDict.Remove(other.Key);
                    }
                }
                else
                {
                    _added.Add(new LangTextDto
                    {
                        Id = Guid.NewGuid(),
                        TextId = other.Value.TextId,
                        IdType = other.Value.IdType,
                        TextEn = other.Value.TextEn,
                        TextZh = other.Value.TextEn,
                        UpdateStats = _compareWindowViewModel.UpdateVersionText,
                        IsTranslated = 0,
                        EnLastModifyTimestamp = DateTime.Now,
                        ZhLastModifyTimestamp = DateTime.Now,
                        LangTextType = other.Value.LangTextType,
                        UserId = App.LangConfig.UserGuid,
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
