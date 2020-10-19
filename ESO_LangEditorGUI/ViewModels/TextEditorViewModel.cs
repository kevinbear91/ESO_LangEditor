using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        //public int TextEditorWindowHeight { get; set; }
        public int TextEditorWindowWidth { get; set; }

        public UC_LangDataGrid LangDataGrid { get; set; }

        public ICommand LangEditorSaveButton { get; }


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

        public string LangTextZh { get; set; }


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

        public TextEditorViewModel(UC_LangDataGrid langdatagrid, List<LangTextDto> langData)
        {
            //_editorList = editorList;

            //_currentLangText = editorList.ElementAt(0);
            

            LangDataGrid = langdatagrid;
            LangDataGrid.TextEditorViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.TextEditorWindow;

            if (langData.Count > 1)
            {
                _gridDataGridHeight = 200;
                _textEditorWindowHeight = 600;
                LangDataGrid.LangDataGrid.Visibility = Visibility.Visible;
                LangDataGrid.LangDataGrid.ItemsSource = langData;
                CurrentLangText = langData.ElementAt(0);
            }
            else
            {
                _gridDataGridHeight = 40;
                _textEditorWindowHeight = 400;
                LangDataGrid.LangDataGrid.Visibility = Visibility.Collapsed;
                CurrentLangText = langData.ElementAt(0);
            }
            
            LangEditorSaveButton = new LangEditorSaveCommand(this);

        }

        public TextEditorViewModel(UC_LangDataGrid langdatagrid, LangTextDto langData)
        {
            LangDataGrid = langdatagrid;
            LangDataGrid.TextEditorViewModel = this;
            LangDataGrid.LangDatGridinWindow = LangDataGridInWindow.TextEditorWindow;

            CurrentLangText = langData;

            LangEditorSaveButton = new LangEditorSaveCommand(this);

        }


    }
}
