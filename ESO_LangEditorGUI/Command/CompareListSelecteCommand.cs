using ESO_LangEditorGUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorGUI.Command
{
    public class CompareListSelecteCommand : CommandBase
    {
        private CompareWindowViewModel _compareWindowViewModel;

        public CompareListSelecteCommand(CompareWindowViewModel compareWindowViewModel)
        {
            _compareWindowViewModel = compareWindowViewModel;
        }


        public override void ExecuteCommand(object parameter)
        {
            //List<LangTextDto> data = parameter as List<LangTextDto>;

            string selectedKey = parameter as string;
            var datagrid = _compareWindowViewModel.LangDataGrid.LangDataGridDC;

            switch (selectedKey)
            {
                case "Added":
                    datagrid.GridData = _compareWindowViewModel.Added;
                    break;
                case "Changed":
                    datagrid.GridData = _compareWindowViewModel.Changed;
                    break;
                case "Removed":
                    datagrid.GridData = _compareWindowViewModel.RemovedList;
                    break;
            }

            //throw new NotImplementedException();
        }
    }
}
