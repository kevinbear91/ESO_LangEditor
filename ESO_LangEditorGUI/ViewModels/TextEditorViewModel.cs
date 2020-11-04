using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class TextEditorViewModel : BaseViewModel
    {
        //private List<LangTextDto> _editorList;
        private LangTextDto _currentLangText;
        private double _gridDataGridHeight;
        private double _textEditorWindowHeight;
        private string _mdNotifyContent;
        private string _langtextInfo;
        private Visibility _dataListVisbility;
        private string _langTextZh;
        private LangTextRepository _langTextRepository = new LangTextRepository();
        private SnackbarMessageQueue _snackbarMessageQueue;
        private List<LangTextDto> _langDataList = new List<LangTextDto>();
        //private LangTextDto> _langDataList;

        //public int TextEditorWindowHeight { get; set; }
        public int TextEditorWindowWidth { get; set; }

        public int CurrentSelectIndex { get; set; }

        public UC_LangDataGrid LangDataGrid { get; set; }

        public ICommand LangEditorSaveButton { get; }

        public event EventHandler OnRequestClose;

        //public List<LangTextDto> EditorList
        //{
        //    get { return _editorList; }
        //    set { _editorList = value; NotifyPropertyChanged(); }
        //}

        public LangTextDto CurrentLangText
        {
            get { return _currentLangText; }
            set { _currentLangText = value; NotifyPropertyChanged(); }
        }

        public double GridDataGridHeight
        {
            get { return _gridDataGridHeight; }
            set { _gridDataGridHeight = value; NotifyPropertyChanged(); }
        }

        public double TextEditorWindowHeight
        {
            get { return _textEditorWindowHeight; }
            set { _textEditorWindowHeight = value; NotifyPropertyChanged(); }
        }

        public string MdNotifyContent
        {
            get { return _mdNotifyContent; }
            set { _mdNotifyContent = value; NotifyPropertyChanged(); }
        }

        public string LangtextInfo
        {
            get { return _langtextInfo; }
            set { _langtextInfo = "文本ID：" + CurrentLangText.TextId + "，文本类型：" + GetIdCategory() + "，" 
                    + "\r\n" + GetVersionName() + " 版本加入或修改，"
                    + "\r\n" + CompareEditTime(); NotifyPropertyChanged(); }
        }

        public Visibility DataListVisbility
        {
            get { return _dataListVisbility; }
            set { _dataListVisbility = value; NotifyPropertyChanged(); }
        }

        public string LangTextZh
        {
            get { return _langTextZh; }
            set { _langTextZh = value; NotifyPropertyChanged(); }
        }

        //public string LangTextZh { get; set; }
        //public Visibility DataListVisbility { get; set; }


        //public TextEditorViewModel(LangTextDto currentLangText)
        //{
        //    _currentLangText = currentLangText;

        //    LangEditorSaveButton = new LangEditorSaveCommand();

        //}

        //public TextEditorViewModel(List<LangTextDto> editorList)
        //{
        //    _editorList = editorList;



        //    _currentLangText = editorList.ElementAt(0);

        //    LangEditorSaveButton = new LangEditorSaveCommand();

        //}

        public TextEditorViewModel(UC_LangDataGrid langdatagrid, List<LangTextDto> langData, SnackbarMessageQueue snackbarMessageQueue)
        {
            //_editorList = editorList;

            //_currentLangText = editorList.ElementAt(0);
            

            LangDataGrid = langdatagrid;
            LangDataGrid.TextEditorViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.TextEditorWindow;

            _langDataList = langData;

            if (_langDataList.Count > 1)
            {
                _gridDataGridHeight = 200;
                _textEditorWindowHeight = 600;
                DataListVisbility = Visibility.Visible;
                LangDataGrid.LangDataGridDC.GridData = _langDataList;
                LangDataGrid.LangDataGridDC.GridSelectedItem = _langDataList.ElementAt(0);
                CurrentLangText = _langDataList.ElementAt(0);
            }
            else
            {
                _gridDataGridHeight = 40;
                _textEditorWindowHeight = 400;
                DataListVisbility = Visibility.Collapsed;
                CurrentLangText = _langDataList.ElementAt(0);
            }
            LangTextZh = CurrentLangText.TextZh;
            LangtextInfo = "update";
            _snackbarMessageQueue = snackbarMessageQueue;
            LangEditorSaveButton = new LangEditorSaveCommand(SaveCurrentToDb);

        }

        public TextEditorViewModel(UC_LangDataGrid langdatagrid, LangTextDto langData, SnackbarMessageQueue snackbarMessageQueue)
        {
            LangDataGrid = langdatagrid;
            LangDataGrid.TextEditorViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.TextEditorWindow;

            CurrentLangText = langData;
            LangTextZh = CurrentLangText.TextZh;

            LangtextInfo = "update";
            DataListVisbility = Visibility.Collapsed;

            _snackbarMessageQueue = snackbarMessageQueue;

            LangEditorSaveButton = new LangEditorSaveCommand(SaveCurrentToDb);

        }

        public void SetCurrentSelectedValue(LangTextDto selectedItem, int selectedIndex)
        {
            if (selectedItem != null & selectedIndex != -1)
            {
                CurrentLangText = selectedItem;
                LangTextZh = CurrentLangText.TextZh;
                CurrentSelectIndex = selectedIndex;
                LangtextInfo = "update";
            }
            
        }

        private string CompareEditTime()
        {
            string resultWord;

            if (CurrentLangText.EnLastModifyTimestamp > CurrentLangText.ZhLastModifyTimestamp)
            {
                if (CurrentLangText.IsTranslated == 1 || CurrentLangText.IsTranslated == 2)
                    resultWord = "英文文本已修改， 翻译可能不匹配。";
                else
                    resultWord = "文本待译。";
            }
            else
            {
                if (CurrentLangText.IsTranslated == 1 || CurrentLangText.IsTranslated == 2)
                    resultWord = "英文文本修改后已翻译。";
                else
                    resultWord = "文本可能为初始化内容。";
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
            if (LangTextZh != CurrentLangText.TextZh)
            {
                CurrentLangText.TextZh = LangTextZh;
                CurrentLangText.IsTranslated = 1;
                CurrentLangText.ZhLastModifyTimestamp = DateTime.Now;
                CurrentLangText.UserId = App.LangConfig.UserGuid;

                Debug.WriteLine("{0},{1}", CurrentLangText.TextZh, CurrentLangText.ZhLastModifyTimestamp);

                int update = await _langTextRepository.UpdateLangZh(CurrentLangText);

                if (update >= 1)
                    MdNotifyContent = "文本ID：" + CurrentLangText.TextId + " 保存成功！";
                else
                    MdNotifyContent = "文本保存失败！";

                _snackbarMessageQueue.Enqueue(MdNotifyContent);

                ModifyDataGridAfterSave();
            }
            else
            {
                MdNotifyContent = "哦豁，没检测到和原来的文本有什么区别，所以没保存。";
                _snackbarMessageQueue.Enqueue(MdNotifyContent);

            }
        }

        private void ModifyDataGridAfterSave()
        {
            if (_langDataList.Count > 1)
            {
                var removeitem = _langDataList.Single(l => l.Id == CurrentLangText.Id);
                Debug.WriteLine(removeitem.Id.ToString());

                _langDataList.Remove(removeitem);

                LangDataGrid.LangDataGridDC.GridData = _langDataList;
                LangDataGrid.LangDataGrid.Items.Refresh();


                if (CurrentSelectIndex > 0)
                    CurrentSelectIndex = CurrentSelectIndex - 1;
                else
                    CurrentSelectIndex = 0;

                SetCurrentSelectedValue(_langDataList.ElementAt(CurrentSelectIndex), CurrentSelectIndex);
            }
            else
            {
                OnRequestClose(this, new EventArgs());
            }
                
        }

    }
}
