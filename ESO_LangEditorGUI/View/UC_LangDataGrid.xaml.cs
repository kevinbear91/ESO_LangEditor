using ESO_LangEditorGUI.Controller;
using ESO_LangEditorGUI.Interface;
using ESO_LangEditorGUI.Models.Enum;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// UC_LangDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class UC_LangDataGrid : UserControl
    {
        WindowController windowControll = new WindowController();
        private ContextMenu menu = new ContextMenu();
        //private DataGridViewModel _dataContext = new DataGridViewModel();

        public UC_LangDataGrid()
        {
            InitializeComponent();
            //DataContext = _dataContext;
        }
        
        public async System.Threading.Tasks.Task SetDataAsync()
        {
            //var vm = new DataGridViewModel();

            //vm.GridData = await windowControll.GetLangTexts();

            //LangDataGrid.ItemsSource = await windowControll.GetLangTexts();

            //SearchLangText search = new SearchLangText();
            
           // _dataContext.GridData = await search.GetLangText(SearchPostion.Full, SearchTextType.ByUser, "148ed451-bf19-43e9-a8d3-55f922cd349e");
             

        }

        private void LangSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            

        }
        private void LangSearchDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // textBlock_SelectionInfo.Text = "已选择 " + LangData.SelectedItems.Count + " 条文本";

        }

        private void LangSearch_MouseRightUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void HeaderClick(object sender, MouseButtonEventArgs e)
        {
            // Fill context menu with column names and checkboxes
            var visibleColumns = LangDataGrid.Columns.Where(c => c.Visibility == Visibility.Visible).Count();

            if(menu.Items.Count > 0)
            {
                menu.Items.Clear();
            }
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
                menu.Items.Add(menuItem);
            }
            // Open it
            menu.IsOpen = true;
        }
    }
}
