using ESO_LangEditor.GUI.ViewModels;
using System.Windows;

namespace ESO_LangEditor.GUI.Views
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
            vm.UserProfileSettingWindow = this;
        }
    }
}
