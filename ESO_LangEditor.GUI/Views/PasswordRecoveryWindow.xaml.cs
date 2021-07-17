using ESO_LangEditor.GUI.ViewModels;
using System.Windows;

namespace ESO_LangEditor.GUI.Views
{
    /// <summary>
    /// PasswordRecoveryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordRecoveryWindow : Window
    {
        public PasswordRecoveryWindow()
        {
            InitializeComponent();

            PasswordRecoveryWindowViewModel _vm = DataContext as PasswordRecoveryWindowViewModel;
            _vm.Load(NewPassword, NewPasswordConfrim);
        }
    }
}
