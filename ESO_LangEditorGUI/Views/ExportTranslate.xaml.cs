using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Views.UserControls;
using System.Windows;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// ExportTranslate.xaml 的交互逻辑
    /// </summary>
    public partial class ExportTranslate : Window
    {

        public ExportTranslate()
        {
            InitializeComponent();
            AddHandler(UC_LangDataGrid.DataGridSelectionChangedEvent,
               new RoutedEventHandler(DataGridSelectionChangedEvent));

            var vm = DataContext as ExportTranslateViewModel;
            vm.OnRequestClose += (s, e) => Close();
        }

        private void DataGridSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as ExportTranslateViewModel;
            DataGridSelectedChangedEventArgs args = (DataGridSelectedChangedEventArgs)e;

            var langtextList = args.LangTextListDto;

            vm.SelectedItems = langtextList;
            vm.SelectedInfo = langtextList.Count.ToString();
        }
        
    }
}
