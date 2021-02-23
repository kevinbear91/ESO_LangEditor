using System;
using System.Windows;
using System.Diagnostics;
using ESO_LangEditorGUI.ViewModels;
using MaterialDesignThemes.Wpf;
using ESO_LangEditorGUI.Services;
using Prism.Events;
using ESO_LangEditorGUI.Views.UserControls;
using ESO_LangEditorGUI.EventAggres;
using System.Windows.Controls;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddHandler(UC_LangDataGrid.DataGridDoubleClick,
                new RoutedEventHandler(DataGridDoubleClickedEvent));

            AddHandler(UC_LangDataGrid.DataGridSelectionChangedEvent,
                new RoutedEventHandler(DataGridSelectionChangedEvent));

            //AddHandler(ExportTranslate.CloseMainWindowDrawerHostEvent,
            //    new RoutedEventHandler(CloseDrawerHostEvent));

            var vm = DataContext as MainWindowViewModel;
            vm.CloseDrawerHostEvent += (s, e) => this.CloseDrawerHostEvent();
            RootDialogWindow.Loaded += vm.RootDialogWindow_Loaded;

        }

        private void DataGridSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;
            DataGridSelectedChangedEventArgs args = (DataGridSelectedChangedEventArgs)e;

            var langtextList = args.LangTextListDto;
            vm.DataGridSelectedCount(langtextList);
        }

        private void DataGridDoubleClickedEvent(object sender, RoutedEventArgs e)
        {

            var vm = DataContext as MainWindowViewModel;
            DataGridSelectedItemEventArgs args = (DataGridSelectedItemEventArgs)e;

            var langtext = args.LangTextDto;
            vm.OpenLangEditor(langtext);

        }

        private void CloseDrawerHostEvent()
        {
            DrawerHostMainWindow.IsTopDrawerOpen = false;
        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
            => Console.WriteLine($"SAMPLE 2: Closing dialog with parameter");

    }
}

