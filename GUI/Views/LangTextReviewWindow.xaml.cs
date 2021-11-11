using Core.Models;
using GUI.Views.UserControls;
using GUI.EventAggres;
using GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Views
{
    /// <summary>
    /// LangTextReviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LangTextReviewWindow : Window
    {
        public LangTextReviewWindow()
        {
            InitializeComponent();
            AddHandler(UC_LangDataGrid.DataGridSelectionChangedEvent,
               new RoutedEventHandler(DataGridSelectionChangedEvent));
        }

        private void DataGridSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LangTextReviewWindowViewModel;
            DataGridReviewSelectedChangedEventArgs args = (DataGridReviewSelectedChangedEventArgs)e;

            var langtextList = args.LangTextListDto;

            vm.GridSelectedItems = langtextList;
            vm.SelectedInfo = langtextList.Count.ToString();
            vm.SetSelectedItemInfo();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as LangTextReviewWindowViewModel;
            var selecteditem = (UserInClientDto)UserListBox.SelectedItem;

            if (selecteditem != null)
            {
                vm.SelectedUser = selecteditem;
                vm.QueryReviewItemsBySelectedUser(sender);
            }


        }
    }
}
