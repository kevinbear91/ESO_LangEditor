using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowOption windowsOptions;
        List<LangSearchModel> d1;

        public MainWindow()
        {
            windowsOptions = new MainWindowOption();
            DataContext = windowsOptions;
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (LangSearch.Items.Count > 1)
                d1 = null;
                LangSearch.Items.Clear();

            d1 = windowsOptions.SearchLang(SearchTextBox.Text);
            foreach (var data in d1)
            {
                LangSearch.Items.Add(data);
            }
        }
    }
}
