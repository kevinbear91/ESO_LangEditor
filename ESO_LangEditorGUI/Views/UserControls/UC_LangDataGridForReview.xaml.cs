using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Converter;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.ViewModels;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ESO_LangEditorGUI.Views.UserControls
{
    /// <summary>
    /// UC_LangDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UC_LangDataGridForReview : UserControl
    {
        //WindowController windowControll = new WindowController();
        private ContextMenu _menu = new ContextMenu();
        public ContextMenu _rowRightClickMenu = new ContextMenu();
        //private LangTextForReviewDto _selectedItem;
        private List<LangTextForReviewDto> _selectedItems;

        private EnumDescriptionConverter _enumDescriptionConverter = new EnumDescriptionConverter();

        public ICommand LangDataGridCommand { get; }

        public IEnumerable<LangDataGridContextMenu> RowRightClickMenuEnum
        {
            get { return Enum.GetValues(typeof(LangDataGridContextMenu)).Cast<LangDataGridContextMenu>(); }
            //set { _searchPostion =  }
        }



        public UC_LangDataGridForReview()
        {
            InitializeComponent();
            //LangDataGridCommand = new LangDataGridCommand(this);

        }

        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            var tag = GetDataGridInWindowTag();

            if (tag == "LangtextReviewWindow")
            {
                _selectedItems = datagrid.SelectedItems.OfType<LangTextForReviewDto>().ToList();
                MakeSelectedItemToEventArgs(_selectedItems);
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

        private List<LangTextForReviewDto> GetSeletedItems()
        {
            var list = new List<LangTextForReviewDto>();

            foreach (var selectedItem in LangDataGrid.SelectedItems)
            {
                if (selectedItem != null)
                    list.Add((LangTextForReviewDto)selectedItem);
            }
            return list;
        }

        private string GetDataGridInWindowTag()
        {
            Window parentWindow = Window.GetWindow(this);

            return parentWindow.Tag.ToString();
        }


        public static readonly RoutedEvent DataGridSelectionChangedEvent =
            EventManager.RegisterRoutedEvent("DataGridSelectChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(UC_LangDataGridForReview));


        public event RoutedEventHandler DataGridSelectChanged
        {
            add { AddHandler(DataGridSelectionChangedEvent, value); }
            remove { RemoveHandler(DataGridSelectionChangedEvent, value); }
        }

        private void MakeSelectedItemToEventArgs(List<LangTextForReviewDto> langtextList)
        {
            DataGridReviewSelectedChangedEventArgs args = new DataGridReviewSelectedChangedEventArgs(DataGridSelectionChangedEvent, langtextList);
            this.RaiseEvent(args);
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
