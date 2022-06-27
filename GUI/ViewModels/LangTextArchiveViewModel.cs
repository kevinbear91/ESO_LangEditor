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
        private string _archivedLangZh;

        public ICommand RollBackLangZhCommand => new ExcuteViewModelMethod(RollBackLangzh);
        public ICommand SetSelectedLangZhToNullCommand => new ExcuteViewModelMethod(SetSelectedLangzhToNull);


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

        //public LangTextDto CurrentSelectedData
        //{
        //    get => _currentSelectedData;
        //    set => SetProperty(ref _currentSelectedData, value);
        //}

        //public LangTextForArchiveDto ArchivedSelectedData
        //{
        //    get => _archivedSelectedData;
        //    set => SetProperty(ref _archivedSelectedData, value);
        //}

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

        public string ArchivedLangZh
        {
            get => _archivedLangZh;
            set => SetProperty(ref _archivedLangZh, value);
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
            //langTextForArchiveWindowItem = _mapper.Map<LangTextForArchiveWindow>(langtext);
            //_currentSelectedData = langtext;

            //var list = await _backendService.GetLangTextInArchived(langtext.TextId);

            //if (list != null && list.Count >= 1)
            //{
            //    if (ArchivedGridData.Count >= 1)
            //    {
            //        ArchivedGridData.Clear();
            //    }
            //    ArchivedGridData.AddRange(list);
            //    CanSelectedArchivedGridData = true;
            //}
            //else
            //{
            //    CanSelectedArchivedGridData = false;
            //}
        }

        private async void RollBackLangzh(object obj)
        {
            //var langArchived = (LangTextForArchiveDto)obj;

            //if (langArchived != null && _currentSelectedData != null)
            //{
            //    GridStatus = "正在上传";

            //    var langUpdate = _mapper.Map<LangTextForUpdateZhDto>(_currentSelectedData);
            //    langUpdate.UserId = App.User.Id;
            //    langUpdate.TextZh = langArchived.TextZh;

            //    await _backendService.UploadlangtextUpdateZh(langUpdate);
            //    GridStatus = "上传完毕";
            //}
        }

        private async void SetSelectedLangzhToNull(object obj)
        {
            //var selectedLangtext = (LangTextDto)obj;

            //GridStatus = "正在上传";
            //var langUpdate = _mapper.Map<LangTextForUpdateZhDto>(selectedLangtext);
            //langUpdate.UserId = App.User.Id;
            //langUpdate.TextZh = null;

            //await _backendService.UploadlangtextUpdateZh(langUpdate);

            //GridStatus = "上传完毕";
        }

        public void SetSelectedArchivedItem(LangTextForArchiveDto langTextForArchiveDto)
        {
            ArchivedLangZh = langTextForArchiveDto.TextZh;
        }
    }
}
