using ESO_LangEditorGUI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static System.Convert;

namespace ESO_LangEditorGUI.Views
{
    /// <summary>
    /// CompareWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CompareWithDBWindow : Window
    {

        //private LangDbController db = new LangDbController();
        public CompareWithDBWindow()
        {
            InitializeComponent();

            var vm = DataContext as CompareWithDBWindowViewModel;
            vm.compareWithDBWindow = this;

            //DataContext = new CompareWindowViewModel(LangDataGrid);

        }


        private async void SaveToDB_Button_Click(object sender, RoutedEventArgs e)
        {
            //string updateStats = VersionInput_textBox.Text;


            //if (updateStats == "" || updateStats == "更新版本号" || updateStats == "更新版本号(必填)")
            //{
            //    MessageBox.Show("请输入新版本文本的版本号！比如“Update25”等！", "提醒",
            //            MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
            //else
            //{
            //    SaveToDB_Button.IsEnabled = false;
            //    SaveToDB_Button.Content = "正在应用更改……";

            //    if (added.Count >= 1)    //判断新加内容是否为空
            //    {
            //        SaveToDB_Button.Content = "正在保存新加内容……";
            //        await Task.Run(() => db.AddNewLangs(added));
            //    }

            //    if (changed.Count >= 1)   //判断修改内容是否为空
            //    {
            //        SaveToDB_Button.Content = "正在应用修改内容……";
            //        await Task.Run(() => db.UpdateLangsEN(changed));
            //    }

            //    if (removedList.Count >= 1)   //判断移除内容是否为空
            //    {
            //        SaveToDB_Button.Content = "正在删除移除内容……";
            //        await Task.Run(() => db.DeleteLangs(removedList));
            //    }

            //}

            //SaveToDB_Button.IsEnabled = true;
            //SaveToDB_Button.Content = "保存";
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
