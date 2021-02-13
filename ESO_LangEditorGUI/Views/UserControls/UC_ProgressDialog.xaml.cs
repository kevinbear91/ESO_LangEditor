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
    /// ProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressDialog : UserControl
    {
        public ProgressDialogViewModel DataContent { get; } = new ProgressDialogViewModel();

        public ProgressDialog()
        {
            InitializeComponent();

            DataContext = DataContent;
        }

    }
}
