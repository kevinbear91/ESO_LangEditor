using ESO_LangEditorGUI.ViewModels;
using System.IO;
using System.Windows;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// PackToRelase.xaml 的交互逻辑
    /// </summary>
    public partial class PackToRelase : Window
    {
        //readonly PackAddonFiles packFiles;

        public PackToRelase()
        {
            DataContext = new PackFileViewModel(this);
            InitializeComponent();
        }

        private bool CheckResFolder()
        {
            if (Directory.Exists("Resources"))
                return true;
            else
                return false;
        }
    }
}
