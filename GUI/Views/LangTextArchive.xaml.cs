using Core.Models;
using GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.Views
{
    /// <summary>
    /// LangTextArchive.xaml 的交互逻辑
    /// </summary>
    public partial class LangTextArchive : Window
    {
        public LangTextArchive(List<LangTextDto> langTextDtoList)
        {
            InitializeComponent();
            var vm = DataContext as LangTextArchiveViewModel;
            vm.Load(langTextDtoList);
        }

        private void LangDataGridList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as LangTextArchiveViewModel;
            var selectedItem = (LangTextDto)LangDataGridList.SelectedItem;

            if (selectedItem != null)
            {
                vm.GetLangtextInArchivedByTextId(selectedItem);
            }

        }

        private void LangDataGridArchive_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as LangTextArchiveViewModel;
            var selectedItem = (LangTextForArchiveDto)LangDataGridArchive.SelectedItem;

            if (selectedItem != null)
            {
                vm.SetSelectedArchivedItem(selectedItem);
            }
        }
    }
}
