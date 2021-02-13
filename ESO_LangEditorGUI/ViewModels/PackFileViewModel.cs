using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditorGUI.Command;
using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.ViewModels
{
    public class PackFileViewModel : BaseViewModel
    {
        private string _addonVersion;
        private string _apiVersion;
        private bool _buttonprogress;
        //private List<FilePaths> _copyFilePaths;
        //private List<FilePaths> _filePaths;

        private PackToRelase _packToRelaseWindow;

        public string AddonVersion
        {
            get { return _addonVersion; }
            set { _addonVersion = value; NotifyPropertyChanged(); }
        }

        public string ApiVersion
        {
            get { return _apiVersion; }
            set { _apiVersion = value; NotifyPropertyChanged(); }
        }

        public bool ButtonProgress
        {
            get { return _buttonprogress; }
            set { _buttonprogress = value; NotifyPropertyChanged(); }
        }

        public CHSorCHT ChsOrChtListSelected { get; set; }

        public IEnumerable<CHSorCHT> ChsOrChtList
        {
            get { return Enum.GetValues(typeof(CHSorCHT)).Cast<CHSorCHT>(); }
        }

        public ExcuteViewModelMethod PackFilesCommand => new ExcuteViewModelMethod(ProcessFilesAsync);


        public PackFileViewModel(PackToRelase packToRelaseWindow)
        {
            //exportToFileCommand = new ExportToFileCommand();
            _packToRelaseWindow = packToRelaseWindow;
        }

        //private void Pack_button_Click()
        //{


        //    if (CheckResFolder())
        //    {
        //        //if (CheckField())
        //        //    packFiles.ProcessFiles(esoZhVersion, apiVersion);
        //        //else
        //        //    MessageBox.Show("汉化版本号与API版本号不得为空！");
        //    }
        //    else
        //    {
        //        MessageBox.Show("无法找到必要文件夹，非开放功能，请群内询问相关问题！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }



        //    //ModifyFiles();
        //    //CopyResList();

        //    //packFiles.ModifyFiles();

        //}

        public async void ProcessFilesAsync(object o)
        {
            var packfile = new PackAllAddonFile(this);

            ButtonProgress = true;
            _packToRelaseWindow.Pack_button.IsEnabled = false;

            await Task.Run(() => packfile.ProcessFiles());

            ButtonProgress = false;
            _packToRelaseWindow.Pack_button.IsEnabled = true;
        }

    }
}
