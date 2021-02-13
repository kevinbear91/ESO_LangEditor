using ESO_LangEditorGUI.ViewModels;
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

namespace ESO_LangEditorGUI.Views.UserControls
{
    /// <summary>
    /// UC_Login.xaml 的交互逻辑
    /// </summary>
    public partial class UC_Login : UserControl
    {
        public LoginViewModel DataContent;

        public UC_Login(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponent();
            DataContent = new LoginViewModel(mainWindowViewModel);
            DataContext = DataContent;
        }
    }
}
