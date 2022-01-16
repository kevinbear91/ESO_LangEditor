using Core.Models;
using GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public LangTextArchiveViewModel(IBackendService backendService)
        {
            _backendService = backendService;


        }

        public async void GetLangtextInArchivedByTextId(string langtextId)
        {
            var list = await _backendService.GetLangTextInArchived(langtextId);

            if (list != null && list.Count >= 1)
            {
                if (ArchivedGridData.Count >= 1)
                {
                    ArchivedGridData.Clear();
                }
                ArchivedGridData.AddRange(list);
            }
        }

        public void SetSelectedArchivedItem(LangTextForArchiveDto langTextForArchiveDto)
        {

        }
    }
}
