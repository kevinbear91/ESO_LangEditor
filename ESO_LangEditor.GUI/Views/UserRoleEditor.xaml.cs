using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ESO_LangEditor.GUI.Views
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

            if (selecteditem != null && vm.IsOpenDialogs == false)
            {
                vm.SelectedUser = selecteditem;
                vm.GetInfoBySelectedUser(sender);
            }
        }

    }
}
