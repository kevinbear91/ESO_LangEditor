using System.Windows.Controls;

namespace ESO_LangEditor.GUI.Views.UserControls
{
    /// <summary>
    /// ProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ImportDbRevProgressDialog : UserControl
    {
        //public ImportDbRevProgressDialogViewModel DataContent { get; }

        public ImportDbRevProgressDialog()
        {
            InitializeComponent();

            //DataContext = new ImportDbRevProgressDialogViewModel(isDbRev);
        }

    }
}
