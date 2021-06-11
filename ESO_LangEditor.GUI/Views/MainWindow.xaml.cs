using System;
using System.Windows;
using System.Diagnostics;
using ESO_LangEditor.GUI.ViewModels;
using MaterialDesignThemes.Wpf;
using ESO_LangEditor.GUI.Services;
using Prism.Events;
using ESO_LangEditor.GUI.Views.UserControls;
using ESO_LangEditor.GUI.EventAggres;
using System.Windows.Controls;
using System.ComponentModel;
using ESO_LangEditor.Core.Models;

namespace ESO_LangEditor.GUI.Views
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
            //RootDialogWindow.IsOpen = true;
            vm.OnRequestClose += (s, e) => this.Close();

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


        protected override void OnClosing(CancelEventArgs e)
        {
            var config = App.LangConfig;
            AppConfigClient.Save(config);

            if (App.LangConfig.AppSetting.IsAskToExit)
            {
                base.OnClosing(e);
                MessageBoxResult result = MessageBox.Show("确定要退出？", "关闭确认", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                e.Cancel = result == MessageBoxResult.Cancel;
            }
            else
            {
                base.OnClosing(e);
            }

        }

        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
            => Console.WriteLine($"SAMPLE 2: Closing dialog with parameter");

    }
}

