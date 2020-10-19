using ESO_LangEditorGUI.Services;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class ExportToFileCommand : CommandBase
    {
        private MainWindowViewModel _mainViewModel;
        private MainWindow _mainWindow;
        private LangTextRepository _langTextRepository = new LangTextRepository();
        private ThirdPartSerices _thirdPartSerices = new ThirdPartSerices();
        private ExportDbToFile exportDbToFile = new ExportDbToFile();

        public ExportToFileCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainViewModel = mainWindowViewModel;
        }

        //public override async Task ExecuteAsync(object parameter)
        //{
        //   await ExportToLang();
        //}

        public override void ExecuteCommand(object parameter)
        {
            //ExportToLang();
        }

        

        private void ToCHT()
        {
            //var export = new ExportFromDB();
            var tolang = new ThirdPartSerices();

            //export.ExportAsText();

            tolang.OpenCCtoCHT();
            tolang.ConvertTxTtoLang(true);

            MessageBox.Show("完成！");
        }
    }
}
