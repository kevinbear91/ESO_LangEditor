using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views.UserControls;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class ExportTranslateViewModel : BindableBase
    {
        private string _searchResultInfo;
        private string _selectedInfo;
        private LangTextDto _selectedItem;
        private List<LangTextDto> _selectedItems;
        private ObservableCollection<LangTextDto> _gridData;
        private bool _isExportSelectedItems;
        private bool _exportEnabled = false;

        private readonly LangTextRepoClientService _langTextSearch = new LangTextRepoClientService();
        public ICommand ExportTranslateCommand => new ExcuteViewModelMethod(ExportTranslatedListAsync);
        public event EventHandler OnRequestClose;
        public event RoutedEventHandler CloseMainWindowDrawerHost;
        IEventAggregator _ea;

        public string SearchResultInfo
        {
            get { return _searchResultInfo; }
            set { SetProperty(ref _searchResultInfo, "总计搜索到 " + value + " 条结果。"); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { SetProperty(ref _selectedInfo, "已选择 " + value + " 条文本。"); }
        }

        public LangTextDto SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        public List<LangTextDto> SelectedItems
        {
            get { return _selectedItems; }
            set { SetProperty(ref _selectedItems, value); }
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get { return _gridData; }
            set { SetProperty(ref _gridData, value); }
        }

        public bool IsExportSelectedItems 
        {
            get { return _isExportSelectedItems; }
            set { SetProperty(ref _isExportSelectedItems, value); }
        }

        public bool ExportEnabled
        {
            get { return _exportEnabled; }
            set { SetProperty(ref _exportEnabled, value); }
        }

        public ExportTranslateViewModel(IEventAggregator ea)
        {
            _ea = ea;

            Task.Run(() => GetTranslatedLangtextList());

        }

        private async void GetTranslatedLangtextList()
        {
            var translatedList = await _langTextSearch.GetLangTextByConditionAsync("1", SearchTextType.TranslateStatus, SearchPostion.Full);
            GridData = new ObservableCollection<LangTextDto>(translatedList);
            SearchResultInfo = GridData.Count.ToString();
            if (GridData.Count >= 1)
                ExportEnabled = true;
        }


        public async void ExportTranslatedListAsync(object o)
        {
            ExportEnabled = false;

            ExportDbToFile exporter = new ExportDbToFile();
            string path;
            List<LangTextDto> list;

            if (IsExportSelectedItems)
            {
                list = SelectedItems;
            }
            else
            {
                list = GridData.ToList();
            }

            path = exporter.ExportLangTextsAsJson(list, LangChangeType.ChangedZH);

            if (await _langTextSearch.UpdateTranslateStatus(list))
            {
                OnRequestClose(this, new EventArgs());
                _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();
                _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Publish("文本保存路径：" + path);
            }
            else
            {
                MessageBox.Show("保存翻译列表状态出错！");
            }
            
        }
    }
}
