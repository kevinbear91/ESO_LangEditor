using GUI.ViewModels;
using System.Windows.Controls;

namespace GUI.Views.UserControls
{
    /// <summary>
    /// UC_Login.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Login : UserControl
    {
        public UC_Login()
        {
            InitializeComponent();

            LoginViewModel _vm = DataContext as LoginViewModel;
            _vm.Load(userPassword);
        }

    }
}
