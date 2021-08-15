using ESO_LangEditor.GUI.ViewModels;
using System.Windows;

namespace ESO_LangEditor.GUI.Views
{
    /// <summary>
    /// RegisterWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            RegisterWindowViewModel _vm = DataContext as RegisterWindowViewModel;
            _vm.Load(Password, Password_Confirm);
        }
    }
}
