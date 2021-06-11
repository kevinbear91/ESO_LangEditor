using ESO_LangEditor.GUI.Services.AccessServer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ESO_LangEditor.GUI.Views.UserControls
{
    /// <summary>
    /// UC_Login.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Login : UserControl
    {
        public UC_Login(AccountService accountService)
        {
            InitializeComponent();

            var _vm = DataContext as LoginViewModel;
            _vm.Load(userPassword, accountService);
        }

    }
}
