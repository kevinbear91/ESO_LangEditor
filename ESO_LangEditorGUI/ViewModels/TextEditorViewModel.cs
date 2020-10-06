using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.View;
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

        public int TextEditorWindowHight { get; set; }
        public int TextEditorWindowWight { get; set; }

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
                LangDataGrid.LangDataGrid.ItemsSource = langData;
                CurrentLangText = langData.ElementAt(0);
            }
            else
            {
                LangDataGrid.LangDataGrid.Visibility = Visibility.Collapsed;
                CurrentLangText = langData.ElementAt(0);
            }
            
            LangEditorSaveButton = new LangEditorSaveCommand();

        }


    }
}
