using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorModels;
using ESO_LangEditorModels.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Command
{
    public class LangDataGridCommand : CommandBaseAsync
    {
        private readonly LangDataGridContextMenu _langdataMenu;
        private readonly List<LangTextDto> _selectedItems;
        private readonly UC_LangDataGrid _langDatagrid;
        //private readonly LangDataGridContextMenu _menuEnum = new LangDataGridContextMenu();

        //public LangDataGridCommand(LangDataGridContextMenu langdataMenu, List<LangTextDto> selectedItems )
        //{
        //    _langdataMenu = langdataMenu;
        //    _selectedItems = selectedItems;
        //}
        public LangDataGridCommand(UC_LangDataGrid langDatagrid)
        {
            _langDatagrid = langDatagrid;
            //_langdataMenu = (LangDataGridContextMenu)langDatagrid._rowRightClickMenu.DataContext;
            //_selectedItems = (List<LangTextDto>)langDatagrid.LangDataGrid.SelectedItems;
        }


        public override Task ExecuteAsync(object parameter)
        {

            LangDataGridContextMenu _menuEnum = (LangDataGridContextMenu)parameter;

            MessageBox.Show(_menuEnum.ToString());

            return null;
            //throw new NotImplementedException();
        }
    }
}
