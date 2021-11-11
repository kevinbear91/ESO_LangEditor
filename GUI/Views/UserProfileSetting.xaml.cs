using GUI.ViewModels;
using System.Windows;

namespace GUI.Views
{
    /// <summary>
    /// UserProfileSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UserProfileSetting : Window
    {
        public UserProfileSetting()
        {
            InitializeComponent();

            var vm = DataContext as UserProfileSettingViewModel;
            vm.Load(PasswordChange, PasswordConfirm, PasswordCurrently);
        }
    }
}
