using GUI.ViewModels;
using System.Windows;

namespace GUI.Views
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWithDBWindow : Window
    {
        public CompareWithDBWindow()
        {
            InitializeComponent();
        }

        private void NewGameVersion_Button_Click(object sender, RoutedEventArgs e)
        {
            var versionWindow = new NewGameVersionWindow
            {
                Owner = this
            };
            versionWindow.Show();
        }
    }
}
