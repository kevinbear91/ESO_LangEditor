using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.ViewModels;
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
    /// UserRoleEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UserRoleEditor : Window
    {
        public UserRoleEditor()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as UserRoleEditorViewModel;
            var selecteditem = (UserInClientDto)UserListBox.SelectedItem;

            if (selecteditem != null)
            {
                vm.SelectedUser = selecteditem;
                vm.GetRolesBySelectedUser(sender);
            }
        }

    }
}
