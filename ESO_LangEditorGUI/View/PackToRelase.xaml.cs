using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using System.Windows;

namespace ESO_LangEditorGUI.View
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
