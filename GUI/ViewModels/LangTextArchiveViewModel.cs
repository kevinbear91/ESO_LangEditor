using AutoMapper;
using Core.Models;
using GUI.Command;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class LangTextArchiveViewModel : BindableBase
    {
        private ObservableCollection<LangTextDto> _currentGridData = new ObservableCollection<LangTextDto>();
        private ObservableCollection<LangTextForArchiveDto> _archivedGridData = new ObservableCollection<LangTextForArchiveDto>();
        private LangTextForArchiveWindow _langTextForArchiveWindowItem;

        private LangTextDto _currentSelectedData;
        private LangTextForArchiveDto _archivedSelectedData;

        private bool _CanSelectedCurrentGridData = true;
        private bool _CanSelectedArchivedGridData = false;

        private string _gridStatus;

        public ICommand RollBackLangZhCommand => new ExcuteViewModelMethod(RollBackLangzh);

        public ObservableCollection<LangTextDto> CurrentGridData
        {
            get => _currentGridData;
            set => SetProperty(ref _currentGridData, value);
        }

        public ObservableCollection<LangTextForArchiveDto> ArchivedGridData
        {
            get => _archivedGridData;
            set => SetProperty(ref _archivedGridData, value);
        }

        public LangTextForArchiveWindow langTextForArchiveWindowItem
        {
            get => _langTextForArchiveWindowItem;
            set => SetProperty(ref _langTextForArchiveWindowItem, value);
        }

        public LangTextDto CurrentSelectedData
        {
            get => _currentSelectedData;
            set => SetProperty(ref _currentSelectedData, value);
        }

        public LangTextForArchiveDto ArchivedSelectedData
        {
            get => _archivedSelectedData;
            set => SetProperty(ref _archivedSelectedData, value);
        }

        public bool CanSelectedCurrentGridData
        {
            get => _CanSelectedCurrentGridData;
            set => SetProperty(ref _CanSelectedCurrentGridData, value);
        }

        public bool CanSelectedArchivedGridData
        {
            get => _CanSelectedArchivedGridData;
            set => SetProperty(ref _CanSelectedArchivedGridData, value);
        }

        public string GridStatus
        {
            get => _gridStatus;
            set => SetProperty(ref _gridStatus, value);
        }

        private IBackendService _backendService;
        private IMapper _mapper;

        public LangTextArchiveViewModel(IBackendService backendService, IMapper mapper)
        {
            _backendService = backendService;
            _mapper = mapper;

        }

        public void Load(List<LangTextDto> langTextDtoList)
        {
            CurrentGridData.AddRange(langTextDtoList);
        }

        public async void GetLangtextInArchivedByTextId(LangTextDto langtext)
        {
            langTextForArchiveWindowItem = _mapper.Map<LangTextForArchiveWindow>(langtext);

            var list = await _backendService.GetLangTextInArchived(langtext.TextId);

            if (list != null && list.Count >= 1)
            {
                if (ArchivedGridData.Count >= 1)
                {
                    ArchivedGridData.Clear();
                }
                ArchivedGridData.AddRange(list);
            }
        }

        private void RollBackLangzh(object obj)
        {
            if (ArchivedSelectedData != null && CurrentSelectedData != null)
            {
                CurrentSelectedData.TextZh = ArchivedSelectedData.TextZh;

                var langUpdate = _mapper.Map<LangTextForUpdateZhDto>(CurrentSelectedData);

                _backendService.UploadlangtextUpdateZh(langUpdate);
            }
        }

        public void SetSelectedArchivedItem(LangTextForArchiveDto langTextForArchiveDto)
        {
            langTextForArchiveWindowItem.TextZhInArchive = langTextForArchiveDto.TextZh;
        }
    }
}
