using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// LangTextReviewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LangTextReviewWindow : Window
    {
        public LangTextReviewWindow()
        {
            InitializeComponent();
            AddHandler(UC_LangDataGridForReview.DataGridSelectionChangedEvent,
               new RoutedEventHandler(DataGridSelectionChangedEvent));
        }

        private void DataGridSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LangTextReviewWindowViewModel;
            DataGridReviewSelectedChangedEventArgs args = (DataGridReviewSelectedChangedEventArgs)e;

            var langtextList = args.LangTextListDto;

            vm.GridSelectedItems = langtextList;
            vm.SelectedInfo = langtextList.Count.ToString();
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
