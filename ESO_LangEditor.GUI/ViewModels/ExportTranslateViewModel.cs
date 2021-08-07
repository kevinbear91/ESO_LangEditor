using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class ExportTranslateViewModel : BindableBase
    {
        private string _searchResultInfo;
        private string _selectedInfo;
        private LangTextDto _selectedItem;
        private List<LangTextDto> _selectedItems;
        private ObservableCollection<LangTextDto> _gridData;
        private LangTextDto _gridSelectedItem;
        private bool _isExportSelectedItems;
        private bool _exportEnabled = false;
        private bool _isNotUpdatedItems;

        private readonly ILangTextRepoClient _langTextSearch;
        public ICommand ExportTranslateCommand => new ExcuteViewModelMethod(ExportTranslatedListAsync);
        public ICommand QueryNotUpdatedLangTextCommand => new ExcuteViewModelMethod(UpdateTranslatedItems_checkBox);

        public event EventHandler OnRequestClose;
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

        public LangTextDto GridSelectedItem
        {
            get { return _gridSelectedItem; }
            set { SetProperty(ref _gridSelectedItem, value); }
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

        public bool IsNotUpdatedItems
        {
            get { return _isNotUpdatedItems; }
            set { SetProperty(ref _isNotUpdatedItems, value); }
        }

        public ExportTranslateViewModel(IEventAggregator ea, ILangTextRepoClient langTextRepoClient)
        {
            _ea = ea;
            _langTextSearch = langTextRepoClient;

            Task.Run(() => GetTranslatedLangtextList());

        }

        private async void UpdateTranslatedItems_checkBox(object obj)
        {
            bool ischecked = (bool)obj;

            if (ischecked)
            {
                await QueryNotUpdatedLangtextAsync();
            }
            else
            {
                await GetTranslatedLangtextList();
            }
        }

        private async Task GetTranslatedLangtextList()
        {
            var translatedList = await _langTextSearch.GetLangTextByConditionAsync("1", SearchTextType.TranslateStatus, SearchPostion.Full);
            GridData = new ObservableCollection<LangTextDto>(translatedList);
            SearchResultInfo = GridData.Count.ToString();
            if (GridData.Count >= 1)
                ExportEnabled = true;
        }

        public async Task QueryNotUpdatedLangtextAsync()
        {
            var translatedList = await _langTextSearch.GetLangTextByConditionAsync("2", SearchTextType.TranslateStatus, SearchPostion.Full);
            GridData = new ObservableCollection<LangTextDto>(translatedList);
            SearchResultInfo = GridData.Count.ToString();
            if (GridData.Count >= 1)
                ExportEnabled = true;
        }


        public async void ExportTranslatedListAsync(object o)
        {
            //ExportEnabled = false;

            //if (IsNotUpdatedItems)
            //{
            //    //var _mapper = App.Mapper;
            //    //var _langTextRepoClient = new LangTextRepoClientService();
            //    var _langTextNetServer = new LangtextNetService(App.ServerPath);
            //    var updateList = _mapper.Map<List<LangTextForUpdateZhDto>>(GridData.ToList());
            //    var code = await _langTextNetServer.UpdateLangtextZh(updateList, App.LangConfig.UserAuthToken);

            //    if (code == System.Net.HttpStatusCode.OK ||
            //    code == System.Net.HttpStatusCode.Accepted ||
            //    code == System.Net.HttpStatusCode.Created)
            //    {

            //        foreach(var lang in updateList)
            //        {
            //            lang.IsTranslated = 3;
            //        }

            //        if(await _langTextRepoClient.UpdateLangtextZh(updateList))
            //        {
            //            OnRequestClose(this, new EventArgs());
            //            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Publish("文本已上传至服务器");
            //        }
            //        else
            //        {
            //            MessageBox.Show("保存翻译列表状态出错！");
            //        }

            //    }
            //    else
            //    {
            //        MessageBox.Show("文本上传至服务器时出错！错误码：" + code);
            //    }

            //}
            //else
            //{
            //    ExportDbToFile exporter = new ExportDbToFile();
            //    string path;
            //    List<LangTextDto> list;

            //    if (IsExportSelectedItems)
            //    {
            //        list = SelectedItems;
            //    }
            //    else
            //    {
            //        list = GridData.ToList();
            //    }

            //    path = exporter.ExportLangTextsAsJson(list, LangChangeType.ChangedZH);

            //    if (await _langTextSearch.UpdateTranslateStatus(list))
            //    {
            //        OnRequestClose(this, new EventArgs());
            //        _ea.GetEvent<CloseMainWindowDrawerHostEvent>().Publish();
            //        _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Publish("文本保存路径：" + path);
            //    }
            //    else
            //    {
            //        MessageBox.Show("保存翻译列表状态出错！");
            //    }
            //}



        }
    }
}
