using ESO_LangEditorGUI.ViewModels;
using System.Windows;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// ExportTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTranslate : Window
    {

        public ExportTranslate()
        {
            InitializeComponent();

            DataContext = new ExportTranslateWindowViewModel(LangDataGrid);
        }


        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
    }
}
