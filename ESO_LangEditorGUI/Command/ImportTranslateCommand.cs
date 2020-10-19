using ESO_LangEditorGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.Command
{
    public class ImportTranslateCommand : CommandBase
    {
        private ImportTranslateWindowViewModel _importTranslateWindowViewModel;

        public ImportTranslateCommand(ImportTranslateWindowViewModel importTranslateWindowViewModel)
        {
            _importTranslateWindowViewModel = importTranslateWindowViewModel;

            if (_importTranslateWindowViewModel.FileList.Count > 1)
                IsExecuting = false;
            else
                IsExecuting = true;
        }


        public override void ExecuteCommand(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
