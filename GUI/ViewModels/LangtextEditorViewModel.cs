using Core.Models;
using GUI;
using GUI.Command;
using GUI.EventAggres;
using GUI.Services;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModels
{
    public class LangtextEditorViewModel : BindableBase
    {
        private LangTextDto _currentLangText;
        private string _langTextZh;
        private Visibility _dataListVisbility;
        private ObservableCollection<LangTextDto> _gridData;
        private LangTextDto _gridSelectedItem;
        private string _mdNotifyContent;
        private bool _isInReview;
        private Visibility _jpVisibility = Visibility.Collapsed;
        private string _langIdTypeName;
        private string _langGameVersionName;
        private string _langLastModifyTime;

        public LangTextDto CurrentLangText
        {
            get => _currentLangText;
            set => SetProperty(ref _currentLangText, value);
        }

        public string LangTextZh
        {
            get => _langTextZh;
            set => SetProperty(ref _langTextZh, value);
        }

        public Visibility DataListVisbility
        {
            get => _dataListVisbility;
            set => SetProperty(ref _dataListVisbility, value);
        }

        public ObservableCollection<LangTextDto> GridData
        {
            get => _gridData;
            set => SetProperty(ref _gridData, value);
        }

        public LangTextDto GridSelectedItem
        {
            get => _gridSelectedItem;
            set => SetProperty(ref _gridSelectedItem, value);
        }

        public string MdNotifyContent
        {
            get => _mdNotifyContent;
            set => SetProperty(ref _mdNotifyContent, value);
        }

        public bool IsInReview
        {
            get => _isInReview;
            set => SetProperty(ref _isInReview, value);
        }

        //public bool IsDisplayJp
        //{
        //    get => App.LangConfig.AppSetting.IsDisplayJp;
        //    set => App.LangConfig.AppSetting.IsDisplayJp = value;
        //}

        public Visibility JpVisibility
        {
            get => _jpVisibility;
            set => SetProperty(ref _jpVisibility, value);
        }

        public string LangIdTypeName
        {
            get => _langIdTypeName;
            set => SetProperty(ref _langIdTypeName, value);
        }

        public string LangGameVersionName
        {
            get => _langGameVersionName;
            set => SetProperty(ref _langGameVersionName, value);
        }

        public string LangLastModifyTime
        {
            get => _langLastModifyTime;
            set => SetProperty(ref _langLastModifyTime, value);
        }

        public Visibility ReasonForVisibility => Visibility.Collapsed;

        //public bool AutoQueryLangTextInReview
        //{
        //    get => App.LangConfig.AppSetting.IsAutoQueryLangTextInReview;
        //    set => App.LangConfig.AppSetting.IsAutoQueryLangTextInReview = value;
        //}

        public ExcuteViewModelMethod LangEditorSaveButton => new ExcuteViewModelMethod(SaveCurrentToDb);
        public SnackbarMessageQueue EditorMessageQueue { get; }
        public event EventHandler OnRequestClose;

        private IEventAggregator _ea;
        //private ILangTextRepoClient _langTextRepoClient;
        //private ILangTextAccess _langTextAccess;
        private IBackendService _backendService;

        public LangtextEditorViewModel(IEventAggregator ea /*ILangTextRepoClient langTextRepoClient, */
            /*ILangTextAccess langTextAccess, IBackendService backendService*/)
        {
            _ea = ea;
            //_langTextRepoClient = langTextRepoClient;
            //_langTextAccess = langTextAccess;
            //_backendService = backendService;
            EditorMessageQueue = new SnackbarMessageQueue();

            _ea.GetEvent<DataGridSelectedItemInEditor>().Subscribe(SetCurrentItemFromList);
        }

        public async void Load(LangTextDto langTextDto)
        {
            CurrentLangText = langTextDto;
            DataListVisbility = Visibility.Collapsed;

            await SetFieldsFromCurrentItem(CurrentLangText);
        }

        public async void Load(List<LangTextDto> langTextDtoList)
        {
            GridData = new ObservableCollection<LangTextDto>(langTextDtoList);

            if (GridData.Count > 1)
            {
                CurrentLangText = GridData.ElementAt(0);
                GridSelectedItem = CurrentLangText;
                DataListVisbility = Visibility.Visible;
            }

            await SetFieldsFromCurrentItem(CurrentLangText);
        }

        private async void SaveCurrentToDb(object o)
        {
            MessageBox.Show("已进入只读模式，无法保存！");
            //if (App.User == null)
            //{
            //    MessageBox.Show("用户无效，无法上传！");
            //}
            //else
            //{
            //    if (LangTextZh != CurrentLangText.TextZh)
            //    {
            //        var time = DateTime.UtcNow;
            //        var langtextUpdateZh = new LangTextForUpdateZhDto
            //        {
            //            Id = CurrentLangText.Id,
            //            TextZh = LangTextZh,
            //            ZhLastModifyTimestamp = time,
            //            UserId = App.User.Id,
            //        };

            //        CurrentLangText.TextZh = LangTextZh;

            //        //await _backendService.UploadlangtextUpdateZh(langtextUpdateZh);
            //        //_ea.GetEvent<UploadLangtextZhUpdateEvent>().Publish(langtextUpdateZh);

            //        if (GridData != null && GridData.Count > 1)
            //        {
            //            GridData.Remove(GridData.Single(l => l.Id == CurrentLangText.Id));

            //            if (GridData.ElementAtOrDefault(0) != null)
            //            {
            //                SetCurrentItemFromList(GridData.ElementAtOrDefault(0));
            //            }
            //        }
            //        else
            //        {
            //            OnRequestClose(this, new EventArgs());
            //            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Publish("文本ID：" + CurrentLangText.TextId + " 保存成功！"
            //                + "需重新搜索才会刷新表格。");
            //        }
            //    }
            //    else
            //    {
            //        EditorMessageQueue.Enqueue("哦豁，没检测到和原来的文本有什么区别，所以没保存。");
            //    }
            //}
        }

        private async Task SetFieldsFromCurrentItem(LangTextDto langTextDto)
        {
            if (langTextDto != null)
            {
                CurrentLangText = langTextDto;
                GridSelectedItem = langTextDto;
                LangTextZh = CurrentLangText.TextZh;
                //LangIdTypeName = await _backendService.GetIdType(langTextDto.IdType);
                //LangGameVersionName = await _backendService.GetGameVersionName(langTextDto.GameApiVersion);

                if (langTextDto.TextZh == null)
                {
                    LangLastModifyTime = langTextDto.EnLastModifyTimestamp.ToString("f");
                }
                else
                {
                    LangLastModifyTime = langTextDto.ZhLastModifyTimestamp.ToString("f");
                }

                if (langTextDto.LangtextInReivewId != null)
                {
                    IsInReview = true;
                }
            }
        }

        public async void SetCurrentItemFromList(LangTextDto langTextDto)
        {
            await SetFieldsFromCurrentItem(langTextDto);
        }
    }
}
