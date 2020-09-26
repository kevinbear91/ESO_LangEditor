using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Models.Enum;
using ESO_LangEditorGUI.View;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace ESO_LangEditorGUI.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly string version = " v3.0.0";

        private string _windowTitle;
        private IEnumerable<SearchPostion> _searchPostion;
        private SearchPostion _selectedSearchPostion;
        private SearchTextType _selectedSearchTextType;
        private string _searchInfo;
        private string _selectedInfo;
        private List<DataGridTextColumn> _dataGridColumnHeader;
        private string _keyword;
        public DataGridViewModel LangDataGridView { get; }

        public UC_LangDataGrid LangDataGrid { get; }

        public ICommand MainviewCommand { get; }

        public ICommand SearchLangCommand { get; }



        public string WindowTitle
        {
            get { return _windowTitle; }
            set { _windowTitle = value; NotifyPropertyChanged(); }
        }

        public SearchPostion SelectedSearchPostion
        {
            get { return _selectedSearchPostion; }
            set { _selectedSearchPostion = value;}
        }

        public IEnumerable<SearchPostion> SearchPostion
        {
            get { return Enum.GetValues(typeof(SearchPostion)).Cast<SearchPostion>(); }
            //set { _searchPostion =  }
        }

        public SearchTextType SelectedSearchTextType
        {
            get { return _selectedSearchTextType; }
            set { _selectedSearchTextType = value; }
        }

        public IEnumerable<SearchTextType> SearchTextType
        {
            get { return Enum.GetValues(typeof(SearchTextType)).Cast<SearchTextType>(); }
            //set { _searchTextType = value; NotifyPropertyChanged(); }
        }

        public string SearchInfo
        {
            get { return _searchInfo; }
            set { _searchInfo = value; NotifyPropertyChanged(); }
        }

        public string SelectedInfo
        {
            get { return _selectedInfo; }
            set { _selectedInfo = value; NotifyPropertyChanged(); }
        }

        public List<DataGridTextColumn> DataGridColumnHeader
        {
            get { return _dataGridColumnHeader; }
            set { _dataGridColumnHeader = value; NotifyPropertyChanged(); }
        }

        public string Keyword
        {
            get { return _keyword; }
            set { _keyword = value; NotifyPropertyChanged(); }
        }



        public MainWindowViewModel()
        {
            LoadMainView();

            //LangDataGrid = new UC_LangDataGrid
            //{
            //    DataContext = _langDataGridView
            //};
            LangDataGridView = new DataGridViewModel();

            SearchLangCommand = new SearchLangCommand(this, LangDataGridView);
            
        }

        private void LoadMainView()
        {
            WindowTitle = "ESO文本查询编辑器" + version;
            SearchInfo = "暂无查询";
            SelectedInfo = "无选择条目";

        }

    }
}
