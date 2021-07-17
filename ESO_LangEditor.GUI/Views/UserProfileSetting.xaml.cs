using ESO_LangEditor.GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
