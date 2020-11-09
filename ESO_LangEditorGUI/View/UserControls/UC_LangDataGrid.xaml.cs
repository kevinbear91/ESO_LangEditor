using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Converter;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorModels;
using ESO_LangEditorModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ESO_LangEditorGUI.View.UserControls
{
    /// <summary>
    /// UC_LangDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UC_LangDataGrid : UserControl
    {
        //WindowController windowControll = new WindowController();
        private ContextMenu _menu = new ContextMenu();
        public ContextMenu _rowRightClickMenu = new ContextMenu();
        private readonly DataGridViewModel _dataContext = new DataGridViewModel();
        private LangTextDto _selectedItem;
        private List<LangTextDto> _selectedItems;

        private EnumDescriptionConverter _enumDescriptionConverter = new EnumDescriptionConverter();

        public MainWindowViewModel MainWindowViewModel { get; set; }
        public TextEditorViewModel TextEditorViewModel { get; set; }
        public ExportTranslateWindowViewModel exportTranslateWindowViewModel { get; set; }

        public LangDataGridInWindow LangDatGridinWindow { get; set; }

        public DataGridViewModel LangDataGridDC { get { return _dataContext; } }

        public ICommand LangDataGridCommand { get;}

        public IEnumerable<LangDataGridContextMenu> RowRightClickMenuEnum
        {
            get { return Enum.GetValues(typeof(LangDataGridContextMenu)).Cast<LangDataGridContextMenu>(); }
            //set { _searchPostion =  }
        }



        public UC_LangDataGrid()
        {
            InitializeComponent();
            LangDataGridCommand = new LangDataGridCommand(this);
            DataContext = _dataContext;
            
        }
        

        private void LangSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid datagrid = sender as DataGrid;

            switch (LangDatGridinWindow)
            {
                case LangDataGridInWindow.MainViewWindow:
                    _selectedItem = (LangTextDto)datagrid.SelectedItem;
                    _dataContext.GridSelectedItem = _selectedItem;
                    MainWindowViewModel.SelectedInfo = "已选择 1 条文本";
                    break;
                //case LangDataGridInWindow.TextEditorWindow:
                //    TextEditorViewModel.CurrentLangText = (LangTextDto)datagrid.SelectedItem;
                //    //Info todo
                //    break;
            }

            TextEditor textEditor = new TextEditor(_selectedItem);
            textEditor.Show();

        }
        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;

            switch(LangDatGridinWindow)
            {
                case LangDataGridInWindow.MainViewWindow:
                    _selectedItems = datagrid.SelectedItems.OfType<LangTextDto>().ToList();
                    _dataContext.GridSelectedItems = _selectedItems;
                    MainWindowViewModel.SelectedInfo = "已选择 " + _selectedItems.Count + " 条文本";
                    break;
                case LangDataGridInWindow.TextEditorWindow:
                    TextEditorViewModel.SetCurrentSelectedValue((LangTextDto)datagrid.SelectedItem, datagrid.SelectedIndex);
                    //TextEditorViewModel.CurrentLangText = (LangTextDto)datagrid.SelectedItem;
                    //TextEditorViewModel.CurrentSelectIndex = datagrid.SelectedIndex;
                    //Info todo
                    break;
                case LangDataGridInWindow.ExportTranslateWindow:
                    exportTranslateWindowViewModel.SelectedItems = datagrid.SelectedItems.OfType<LangTextDto>().ToList();
                    break;

            }
        }


        private void LangSearch_MouseRightUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void LangDataGridRightClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;
            //target = VisualTreeHelper.GetParent(target);

            while (target != null)
            {
                if (target is DataGridColumnHeader)
                {
                    HeaderRightClickMenuGenerater();
                }
                else if (target is DataGridRow)
                {
                    if (datagrid.SelectedIndex != -1)
                    {
                        RowRightClickMenuGenerater();
                    }
                }

                //MessageBox.Show(target.ToString());
                target = VisualTreeHelper.GetParent(target);
            }

        }

        private void HeaderRightClickMenuGenerater()
        {
            var visibleColumns = LangDataGrid.Columns.Where(c => c.Visibility == Visibility.Visible).Count();

            if (_menu.Items.Count == 0)
            {
                foreach (var column in LangDataGrid.Columns)
                {
                    var menuItem = new MenuItem
                    {
                        Header = column.Header.ToString(),
                        IsChecked = column.Visibility == Visibility.Visible,
                        IsCheckable = true,
                        // Don't allow user to hide all columns
                        IsEnabled = visibleColumns > 1 || column.Visibility != Visibility.Visible
                    };
                    // Bind events
                    menuItem.Checked += (object a, RoutedEventArgs ea)
                        => column.Visibility = Visibility.Visible;
                    menuItem.Unchecked += (object b, RoutedEventArgs eb)
                        => column.Visibility = Visibility.Collapsed;
                    _menu.Items.Add(menuItem);
                }
            }
            else
            {
                foreach (MenuItem item in _menu.Items)
                {
                    if (visibleColumns == 1 && item.IsChecked == true)
                    {
                        item.IsEnabled = false;
                    }
                    else
                    {
                        item.IsEnabled = true;
                    }
                }
            }
            _menu.IsOpen = true;
        }

        private void RowRightClickMenuGenerater()
        {

            if (_rowRightClickMenu.Items.Count == 0)
            {
                //_rowRightClickMenu.ItemsSource = RowRightClickMenuEnum;

                //foreach (MenuItem item in _rowRightClickMenu.Items)
                //{
                //    item.Header = _enumDescriptionConverter.GetEnumDescription((LangDataGridContextMenu)item.DataContext);
                //    item.Click += RowRightClickMenu_OnClick;
                //}

                foreach (var item in RowRightClickMenuEnum)
                {
                    var menuItem = new MenuItem
                    {
                        Header = _enumDescriptionConverter.GetEnumDescription(item),
                        DataContext = item,
                        //IsChecked = column.Visibility == Visibility.Visible,
                        //IsCheckable = true,
                        // Don't allow user to hide all columns
                        //IsEnabled = visibleColumns > 1 || column.Visibility != Visibility.Visible
                        //Command = LangDataGridCommand,
                        //CommandParameter = item,
                        //ItemsSource = RowRightClickMenuEnum,

                    };
                    menuItem.Click += RowRightClickMenu_OnClick;
                    //(object a, RoutedEventArgs ea)
                    //=> MessageBox.Show(menuItem.DataContext.ToString());
                    _rowRightClickMenu.Items.Add(menuItem);
                }
            }
            

            _rowRightClickMenu.IsOpen = true;
        }

        private void RowRightClickMenu_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            //ContextMenu cm = mi.Parent as ContextMenu;
            LangDataGridContextMenu _menuEnum = (LangDataGridContextMenu)menuitem.DataContext;

            switch (_menuEnum)
            {
                //LangDataGridContextMenu.EditCurrentItem => "编辑当前",
                case LangDataGridContextMenu.EditMutilItem:
                    new TextEditor(GetSeletedItems()).Show();
                    break;
                case LangDataGridContextMenu.SearchAndReplace:
                    new TextEditor_SearchReplace(_selectedItems).Show();
                    break;

                    //LangDataGridContextMenu.SearchAndReplace => "查找替换",

            };

            //MessageBox.Show(word);

        }

        private List<LangTextDto> GetSeletedItems()
        {
            var list = new List<LangTextDto>();

            foreach (var selectedItem in LangDataGrid.SelectedItems)
            {
                if (selectedItem != null)
                    list.Add((LangTextDto)selectedItem);
            }
            return list;
        }

        //private void OpenLangEditorWindow()
        //{
        //    //List<LangTextDto> selectedItems = (List<LangTextDto>)LangDataGrid.SelectedItems;

        //    TextEditor langEditor = new TextEditor(_selectedItems);

        //    //langEditor.UpdateData(_selectedItems);
        //    langEditor.Show();
        //}
    }
}
