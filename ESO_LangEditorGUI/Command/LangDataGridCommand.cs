using ESO_LangEditorGUI.View.UserControls;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
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
            //UC_LangDataGrid _langDatagrid = parameter as UC_LangDataGrid;

            //object value = (object[])parameter;
            //LangTextDto selectedItems = parameter as LangTextDto;
            //IList selectedItems = (IList)value[0];
            //LangDataGridContextMenu _langdataMenu = value[1] as LangDataGridContextMenu;



            //string boxstring = _langdataMenu switch
            // {
            //     LangDataGridContextMenu.EditMutilItem => "已选择了{0}个项",

            // };_langDatagrid.LangDataGrid.SelectedItems.Count.ToString(),

            LangDataGridContextMenu _menuEnum = (LangDataGridContextMenu)parameter;



            MessageBox.Show(_menuEnum.ToString());

            return null;
            //throw new NotImplementedException();
        }
    }
}
