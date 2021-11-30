using Core.EnumTypes;
using Core.Models;
using GUI.Converter;
using GUI.EventAggres;
using GUI.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace GUI.Views.UserControls
{
    /// <summary>
    /// UC_LangDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UC_LangDataGrid : UserControl
    {
        //WindowController windowControll = new WindowController();
        private ContextMenu _menu = new ContextMenu();
        public ContextMenu _rowRightClickMenu = new ContextMenu();
        private LangTextDto _selectedItem;
        private List<LangTextDto> _selectedItems;
        private List<LangTextForReviewDto> _selectedItemsForReview;

        private EnumDescriptionConverter _enumDescriptionConverter = new EnumDescriptionConverter();

        public ICommand LangDataGridCommand { get; }

        public IEnumerable<LangDataGridContextMenu> RowRightClickMenuEnum
        {
            get { return Enum.GetValues(typeof(LangDataGridContextMenu)).Cast<LangDataGridContextMenu>(); }
            //set { _searchPostion =  }
        }

        public UC_LangDataGrid()
        {
            InitializeComponent();
        }

        private void LangSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGrid datagrid = sender as DataGrid;
            Point aP = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(aP);
            DependencyObject target = obj as DependencyObject;

            while (target != null)
            {
                if (target is DataGridRow & GetDataGridInWindowTag() == "Mainwindow")
                {
                    if (datagrid.SelectedIndex != -1)
                    {
                        _selectedItem = (LangTextDto)datagrid.SelectedItem;
                        MakeDouleClickSelectedItemToEventArgs(_selectedItem);
                    }
                }
                target = VisualTreeHelper.GetParent(target);
            }


        }
        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            var tag = GetDataGridInWindowTag();

            if (tag == "Mainwindow")
            {
                _selectedItems = datagrid.SelectedItems.OfType<LangTextDto>().ToList();
                MakeSelectedItemToEventArgs(_selectedItems);
            }
            else if (tag == "LangtextEditor")
            {
                _selectedItem = (LangTextDto)datagrid.SelectedItem;
                MakeSelectedItemToEditorEventArgs(_selectedItem);
            }
            else if (tag == "LangtextReviewWindow")
            {
                _selectedItemsForReview = datagrid.SelectedItems.OfType<LangTextForReviewDto>().ToList();
                MakeSelectedItemToEventArgs(_selectedItemsForReview);
            }
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
                else if (target is DataGridRow & GetDataGridInWindowTag() == "Mainwindow")
                {
                    if (datagrid.SelectedIndex != -1)
                    {
                        RowRightClickMenuGenerater();
                    }
                }
                //else if (target is DataGridRow & GetDataGridInWindowTag() == "LangtextReviewWindow")
                //{
                //    if (datagrid.SelectedIndex != -1)
                //    {
                //        if (_rowRightClickMenu.Items.Count == 0)
                //        {
                //            foreach (var item in RowRightClickMenuEnum)
                //            {
                //                if (item == LangDataGridContextMenu.DeleteUnderReview)
                //                {
                //                    var menuItem = new MenuItem
                //                    {
                //                        Header = _enumDescriptionConverter.GetEnumDescription(item),
                //                        DataContext = item,
                //                    };
                //                    menuItem.Click += RowRightClickMenu_OnClick;
                //                    _rowRightClickMenu.Items.Add(menuItem);
                //                }
                //            }
                //        }
                //        _rowRightClickMenu.IsOpen = true;
                //    }
                //}
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
                    MenuItem menuItem;
                    if (GetDataGridInWindowTag() != "LangtextReviewWindow" && column.Header.ToString() == "变更原因")
                    {
                        menuItem = new MenuItem
                        {
                            Header = column.Header.ToString(),
                            IsChecked = column.Visibility == Visibility.Visible,
                            IsCheckable = false,
                            // Don't allow user to hide all columns
                            IsEnabled = false
                        };
                    }
                    else
                    {
                        menuItem = new MenuItem
                        {
                            Header = column.Header.ToString(),
                            IsChecked = column.Visibility == Visibility.Visible,
                            IsCheckable = true,
                            // Don't allow user to hide all columns
                            IsEnabled = visibleColumns > 1 || column.Visibility != Visibility.Visible
                        };
                    }

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

                    if (GetDataGridInWindowTag() != "LangtextReviewWindow" && item.Header.ToString() == "变更原因")
                    {
                        item.IsEnabled = false;
                    }
                }
            }
            _menu.IsOpen = true;
        }

        private void RowRightClickMenuGenerater()
        {
            if (_rowRightClickMenu.Items.Count == 0)
            {
                foreach (var item in RowRightClickMenuEnum)
                {
                    //if (item != LangDataGridContextMenu.DeleteUnderReview)
                    //{
                        
                    //}
                    var menuItem = new MenuItem
                    {
                        Header = _enumDescriptionConverter.GetEnumDescription(item),
                        DataContext = item,
                    };
                    menuItem.Click += RowRightClickMenu_OnClick;
                    _rowRightClickMenu.Items.Add(menuItem);
                }
            }
            _rowRightClickMenu.IsOpen = true;
        }
        //private void RowRightClickMenuGenerater_ReviewWindow()
        //{
        //    if (_rowRightClickMenu.Items.Count == 0)
        //    {
        //        foreach (var item in RowRightClickMenuEnum)
        //        {
        //            var menuItem = new MenuItem
        //            {
        //                Header = _enumDescriptionConverter.GetEnumDescription(item),
        //                DataContext = item,
        //            };
        //            menuItem.Click += RowRightClickMenu_OnClick;
        //            _rowRightClickMenu.Items.Add(menuItem);
        //        }
        //    }
        //    _rowRightClickMenu.IsOpen = true;
        //}

        private void RowRightClickMenu_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            LangDataGridContextMenu _menuEnum = (LangDataGridContextMenu)menuitem.DataContext;

            switch (_menuEnum)
            {
                case LangDataGridContextMenu.EditMutilItem:
                    new LangtextEditor(GetSeletedItems()).Show();
                    break;
                case LangDataGridContextMenu.SearchAndReplace:
                    new SearchReplaceWindow(GetSeletedItems()).Show();
                    break;
                //case LangDataGridContextMenu.DeleteUnderReview:
                //    var context = this.DataContext as LangTextReviewWindowViewModel;
                //    context.SubmitDeleteSelectedItemsToServer();
                //    break;
            };

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

        private string GetDataGridInWindowTag()
        {
            Window parentWindow = Window.GetWindow(this);

            return parentWindow.Tag.ToString();
        }

        public static readonly RoutedEvent DataGridDoubleClick =
            EventManager.RegisterRoutedEvent("DataGridDoubleClick", RoutingStrategy.Direct,
                typeof(RoutedEventHandler), typeof(UC_LangDataGrid));


        public event RoutedEventHandler DataGridDoubleClicked
        {
            add { AddHandler(DataGridDoubleClick, value); }
            remove { RemoveHandler(DataGridDoubleClick, value); }
        }


        public static readonly RoutedEvent DataGridSelectionChangedEvent =
            EventManager.RegisterRoutedEvent("DataGridSelectChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UC_LangDataGrid));


        public event RoutedEventHandler DataGridSelectChanged
        {
            add { AddHandler(DataGridSelectionChangedEvent, value); }
            remove { RemoveHandler(DataGridSelectionChangedEvent, value); }
        }

        private void MakeSelectedItemToEventArgs(List<LangTextDto> langtextList)
        {
            DataGridSelectedChangedEventArgs args = new DataGridSelectedChangedEventArgs(DataGridSelectionChangedEvent, langtextList);
            this.RaiseEvent(args);
        }

        private void MakeSelectedItemToEventArgs(List<LangTextForReviewDto> langtextList)
        {
            DataGridReviewSelectedChangedEventArgs args = new DataGridReviewSelectedChangedEventArgs(DataGridSelectionChangedEvent, langtextList);
            this.RaiseEvent(args);
        }

        private void MakeDouleClickSelectedItemToEventArgs(LangTextDto langtext)
        {
            DataGridSelectedItemEventArgs args = new DataGridSelectedItemEventArgs(DataGridDoubleClick, langtext);
            this.RaiseEvent(args);
        }
        private void MakeSelectedItemToEditorEventArgs(LangTextDto langtext)
        {
            DataGridSelectedItemEventArgs args = new DataGridSelectedItemEventArgs(DataGridSelectionChangedEvent, langtext);
            this.RaiseEvent(args);
        }

    }
}
