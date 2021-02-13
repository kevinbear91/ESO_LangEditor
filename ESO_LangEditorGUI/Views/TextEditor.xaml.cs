using ESO_LangEditor.Core.Models;
using ESO_LangEditorGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {


        //private LangDbController db = new LangDbController();

        private bool _isTextZhChanged = false;

        private TextEditorViewModel _dataContext { get; }

        public DataGridViewModel LangDataGridView { get; }

        private bool isLua;

        MainWindow Mainwindow;

        public TextEditor(List<LangTextDto> langData)
        {

            InitializeComponent();
            //_dataContext = new TextEditorViewModel(LangDataGrid, langData, Snackbar_SaveInfo.MessageQueue);
            //DataContext = _dataContext;
            this.Height = 400;


            Closed += TextEditorWindow_Closed;

            isLua = false;

            _dataContext.OnRequestClose += (s, e) => this.Close();


        }

        public TextEditor(LangTextDto langData)
        {

            InitializeComponent();
            //_dataContext = new TextEditorViewModel(LangDataGrid, langData, Snackbar_SaveInfo.MessageQueue);
            //DataContext = _dataContext;
            this.Height = 400;


            Closed += TextEditorWindow_Closed;
            _dataContext.OnRequestClose += (s, e) => this.Close();

            isLua = false;


        }

        private void TextEditorWindow_Closed(object sender, EventArgs e)
        {
            //MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            //viewModel.SaveSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            if(_dataContext.LangTextZh != _dataContext.CurrentLangText.TextZh)
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


        private void Button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentLangText.TextZh = textBox_ZH.Text;
            _isTextZhChanged = false;
        }


    }
}
