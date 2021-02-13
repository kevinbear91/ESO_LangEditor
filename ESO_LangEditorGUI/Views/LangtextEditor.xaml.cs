using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.EventAggres;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorGUI.Views.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// LangtextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class LangtextEditor : Window
    {
        public LangtextEditor(LangTextDto langTextDto)
        {
            InitializeComponent();

            var vm = DataContext as LangtextEditorViewModel;
            vm.Load(langTextDto);

            //Closed += TextEditorWindow_Closed;

            vm.OnRequestClose += (s, e) => this.Close();

        }

        public LangtextEditor(List<LangTextDto> langTextDtoList)
        {
            InitializeComponent();

            var vm = DataContext as LangtextEditorViewModel;
            vm.Load(langTextDtoList);

            AddHandler(UC_LangDataGrid.DataGridSelectionChangedEvent,
                new RoutedEventHandler(DataGridSelectionChangedEvent));
        }

        private void DataGridSelectionChangedEvent(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as LangtextEditorViewModel;
            DataGridSelectedItemEventArgs args = (DataGridSelectedItemEventArgs)e;

            var langtextDto = args.LangTextDto;
            vm.SetCurrentItemFromList(langtextDto);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = DataContext as LangtextEditorViewModel;
            if (vm.LangTextZh != vm.CurrentLangText.TextZh)
            {
                base.OnClosing(e);
                MessageBoxResult result = MessageBox.Show("确定要关闭窗口？当前文本修改后未保存。", "关闭确认", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                e.Cancel = result == MessageBoxResult.Cancel;
            }
            else
            {
                base.OnClosing(e);
            }

        }

    }
}
