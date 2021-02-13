using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditorGUI.Command;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainWindowSearchbarViewModel : BindableBase
    {

        private SearchPostion _selectedSearchPostion;
        private SearchTextType _selectedSearchTextType;
        private SearchTextType _selectedSearchTextTypeSecond;
        private string _keyword;
        private string _keywordSecond;
        private int _mainSearchBarWidth = 550;
        private int _secondSearchBarWidth = 500;
        private Visibility _secondComboxAndSearchBarVisibility = Visibility.Collapsed;
        private bool _secondComboxAndSearchBarEnable = false;


        public ICommand SearchLangCommand { get; }

        public SearchPostion SelectedSearchPostion 
        {
            get { return _selectedSearchPostion; } 
            set { SetProperty(ref _selectedSearchPostion, value); }
        }

        public IEnumerable<SearchPostion> SearchPostionCombox
        {
            get { return Enum.GetValues(typeof(SearchPostion)).Cast<SearchPostion>(); }
            //set { _searchPostion =  }
        }

        public IEnumerable<SearchTextType> SearchTextTypeCombox
        {
            get { return Enum.GetValues(typeof(SearchTextType)).Cast<SearchTextType>(); }
            //set { _searchTextType = value; NotifyPropertyChanged(); }
        }

        public SearchTextType SelectedSearchTextType 
        {
            get { return _selectedSearchTextType; }
            set { SetProperty(ref _selectedSearchTextType, value); SetSecondOptionVisiable(); }
        }

        public SearchTextType SelectedSearchTextTypeSecond
        {
            get { return _selectedSearchTextTypeSecond; }
            set { SetProperty(ref _selectedSearchTextTypeSecond, value); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { SetProperty(ref _keyword, value); }
        }

        public string KeywordSecond
        {
            get { return _keywordSecond; }
            set { SetProperty(ref _keywordSecond, value); }
        }

        public int MainSearchBarWidth
        {
            get { return _mainSearchBarWidth; }
            set { SetProperty(ref _mainSearchBarWidth, value); }
        }

        public int SecondSearchBarWidth
        {
            get { return _secondSearchBarWidth; }
            set { SetProperty(ref _secondSearchBarWidth, value); }
        }

        public Visibility SecondComboxAndSearchBarVisibility
        {
            get { return _secondComboxAndSearchBarVisibility; }
            set { SetProperty(ref _secondComboxAndSearchBarVisibility, value); }
        }

        public bool SecondComboxAndSearchBarEnable
        {
            get { return _secondComboxAndSearchBarEnable; }
            set { SetProperty(ref _secondComboxAndSearchBarEnable, value); }
        }


        private void SetSecondOptionVisiable()
        {

            if (CanShowSecondOption())
            {
                MainSearchBarWidth = 300;
                SecondComboxAndSearchBarVisibility = Visibility.Visible;
                SecondComboxAndSearchBarEnable = true;
            }
            else
            {
                MainSearchBarWidth = 550;
                SecondComboxAndSearchBarVisibility = Visibility.Collapsed;
                SecondComboxAndSearchBarEnable = false;
            }

        }


        private bool CanShowSecondOption()
        {

            if (SelectedSearchTextType == SearchTextType.Type)
                return true;

            return false;

        }

        public MainWindowSearchbarViewModel(IEventAggregator ea)
        {
            SearchLangCommand = new SearchLangCommand(this, ea);
        }


        //protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}


    }
}
