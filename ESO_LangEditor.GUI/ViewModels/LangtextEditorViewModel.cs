using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.Command;
using ESO_LangEditor.GUI.Services;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Prism.Events;
using ESO_LangEditor.GUI.EventAggres;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace ESO_LangEditor.GUI.ViewModels
{
    public class LangtextEditorViewModel : BindableBase
    {
        private LangTextDto _currentLangText;
        private string _langtextInfo;
        private string _langTextZh;
        private Visibility _dataListVisbility;
        private ObservableCollection<LangTextDto> _gridData;
        private LangTextDto _gridSelectedItem;
        private string _mdNotifyContent;
        private int _currentSelectedIndex = 0;

        public LangTextDto CurrentLangText
        {
            get { return _currentLangText; }
            set { SetProperty(ref _currentLangText, value); }
        }

        public string LangTextZh
        {
            get { return _langTextZh; }
            set { SetProperty(ref _langTextZh, value); }
        }

        public Visibility DataListVisbility
        {
            get { return _dataListVisbility; }
            set { SetProperty(ref _dataListVisbility, value); }
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


        public string MdNotifyContent
        {
            get { return _mdNotifyContent; }
            set { SetProperty(ref _mdNotifyContent, value); }
        }

        public string LangtextInfo
        {
            get { return _langtextInfo; }
            set
            {
                SetProperty(ref _langtextInfo, "ID：" + CurrentLangText.TextId + "，类型：" + GetIdCategory() + "，"
                   + GetVersionName() + " 版本加入或修改，"
                   + CompareEditTime());
            }
        }

        IEventAggregator _ea;
        public ICommand LangEditorSaveButton { get; }
        public SnackbarMessageQueue EditorMessageQueue { get; }
        public event EventHandler OnRequestClose;

        private readonly LangTextRepoClientService _langTextRepoClient = new LangTextRepoClientService();

        public LangtextEditorViewModel(IEventAggregator ea)
        {
            _ea = ea;
            LangEditorSaveButton = new ExcuteViewModelMethod(SaveCurrentToDb);
            EditorMessageQueue = new SnackbarMessageQueue();

            _ea.GetEvent<DataGridSelectedItemInEditor>().Subscribe(SetCurrentItemFromList);

        }


        public void Load(LangTextDto langTextDto)
        {
            CurrentLangText = langTextDto;
            LangTextZh = langTextDto.TextZh;
            LangtextInfo = "update";
            DataListVisbility = Visibility.Collapsed;
        }

        public void Load(List<LangTextDto> langTextDtoList)
        {
            GridData = new ObservableCollection<LangTextDto>(langTextDtoList);

            if (GridData.Count > 1)
            {
                CurrentLangText = GridData.ElementAt(0);
                GridSelectedItem = CurrentLangText;
                DataListVisbility = Visibility.Visible;

            }
            LangTextZh = CurrentLangText.TextZh;
            LangtextInfo = "update";
        }

        private string CompareEditTime()
        {
            string resultWord;

            if (CurrentLangText.EnLastModifyTimestamp > CurrentLangText.ZhLastModifyTimestamp)
            {
                resultWord = "英文已修改， 翻译可能不匹配或待译。";
            }
            else
            {
                if (CurrentLangText.IsTranslated != 0)
                {
                    resultWord = "英文修改后已翻译。";
                }
                else
                {
                    resultWord = "文本可能为初始化内容。";
                }
            }
            return resultWord;
        }

        private string GetVersionName()
        {
            return App.gameUpdateVersionName.GetVersionName(CurrentLangText.UpdateStats);
        }

        private string GetIdCategory()
        {
            return App.iDCatalog.GetCategory(CurrentLangText.IdType);
        }

        private async void SaveCurrentToDb(object o)
        {
            var user = await _langTextRepoClient.GetUserInClient(App.LangConfig.UserGuid);

            if(user == null)
            {
                MessageBox.Show("用户无效，保存失败！");
            }
            else
            {
                if (LangTextZh != CurrentLangText.TextZh)
                {
                    var time = DateTime.UtcNow;
                    //time = new DateTime(time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second, time.Kind);
                    var langtextUpdateZh = new LangTextForUpdateZhDto
                    {
                        Id = CurrentLangText.Id,
                        TextZh = LangTextZh,
                        IsTranslated = 1,
                        ZhLastModifyTimestamp = time,
                        UserId = user.Id,
                    };

                    //Debug.WriteLine("{0},{1}", CurrentLangText.TextZh, CurrentLangText.ZhLastModifyTimestamp);

                    if (await _langTextRepoClient.UpdateLangtextZh(langtextUpdateZh))
                    {
                        CurrentLangText.TextZh = LangTextZh;

                        if (GridData != null && GridData.Count > 1)
                        {
                            var removeitem = GridData.Single(l => l.Id == CurrentLangText.Id);
                            GridData.Remove(removeitem);


                            SetCurrentItemFromList(GridData.ElementAtOrDefault(0));

                        }
                        else
                        {
                            OnRequestClose(this, new EventArgs());
                            _ea.GetEvent<SendMessageQueueToMainWindowEventArgs>().Publish("文本ID：" + CurrentLangText.TextId + " 保存成功！"
                                + "需重新搜索才会刷新表格。");
                            _ea.GetEvent<UploadLangtextZhUpdateEvent>().Publish(langtextUpdateZh);
                        }

                    }
                    else
                    {
                        MdNotifyContent = "文本保存失败！";
                    }
                }
                else
                {
                    EditorMessageQueue.Enqueue("哦豁，没检测到和原来的文本有什么区别，所以没保存。");
                }
            }


            
        }

        public void SetCurrentItemFromList(LangTextDto langTextDto)
        {
            if (langTextDto != null)
            {
                Debug.WriteLine(langTextDto.TextZh);

                CurrentLangText = langTextDto;
                GridSelectedItem = langTextDto;
                LangTextZh = CurrentLangText.TextZh;
                LangtextInfo = "update";
            }
            
        }

    }
}
