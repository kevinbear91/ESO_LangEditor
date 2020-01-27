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
using ESO_Lang_Editor.Model;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        private LangSearchModel EditData;
        private List<LangSearchModel> SelectedItems;
        private int selectedListIndex;

        IDCatalog IDtypeName = new IDCatalog();

        public TextEditor(LangSearchModel LangData, List<LangSearchModel> SelectedItemsList)
        {
            
            InitializeComponent();
            this.Width = 530;
          
            SelectedItems = SelectedItemsList;

            List_Datagrid_Display(LangData);

        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            //var EditedData = new LangSearchModel();
            var connDB = new SQLiteController();
            connDB.ConnectTranslateDB();

            if (SaveToMainDB_checkBox.IsChecked == true)
            {
                var EditedData = SetEditedData();
                var updateResult = connDB.UpdateDataFromEditor(EditedData);

                MessageBox.Show(updateResult);

            }
            else
            {
                if (connDB.CheckTableIfExist(EditData.ID_Table))
                {
                    var EditedData = SetEditedData();
                    var updateResult = connDB.AddOrUpdateDataFromEditor(EditedData);

                    MessageBox.Show(updateResult);
                }
                else
                {
                    connDB.CreateTableToTranselateDB(EditData.ID_Table);

                    var EditedData = SetEditedData();
                    var updateResult = connDB.AddOrUpdateDataFromEditor(EditedData);

                    MessageBox.Show(updateResult);
                }

            }

            if (List_dataGrid.Items.Count > 1)
            {
                List_dataGrid.Items.RemoveAt(selectedListIndex);
            }
            else
            {
                this.Close();
            }
        }

        private void List_expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Width = 750;
            List_expander.Header = "收起列表";
        }

        private void List_expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Width = 530;
            List_expander.Header = "展开列表";
        }

        private void List_Datagrid_Display(LangSearchModel LangData)
        {
            //var IDtypeName = new IDCatalog();

            if (List_dataGrid.Items.Count > 1)
                //SearchData = null;
                List_dataGrid.Items.Clear();

            //SearchData = SearchLang(SearchCheck());
            if (SelectedItems != null && SelectedItems.Count > 1)
            {
                List_expander.Visibility = Visibility.Visible;
                List_expander.IsExpanded = true;

                foreach (var item in SelectedItems)
                {
                    List_dataGrid.Items.Add(item);
                }
                List_dataGrid.SelectedIndex = 0;

                //EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);

                SetEditDataTextBlocks(SelectedItems.ElementAt(List_dataGrid.SelectedIndex));

            }
            else
            {
                List_expander.Visibility = Visibility.Hidden;
                List_expander.IsExpanded = false;
                //EditData = LangData;
                SetEditDataTextBlocks(LangData);

 
            }

            //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        }

        private void List_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var IDtypeName = new IDCatalog();
            DataGrid datagrid = sender as DataGrid;
 
            if (datagrid.SelectedIndex != -1)
            {
                SetEditDataTextBlocks(SelectedItems.ElementAt(List_dataGrid.SelectedIndex));
                selectedListIndex = List_dataGrid.SelectedIndex;
            }


        }

        private void SetEditDataTextBlocks(LangSearchModel Data)
        {
            EditData = Data;

            textBox_EN.Text = Data.Text_EN;
            textBox_ZH.Text = Data.Text_SC;
            textblock_information.Text = "当前表：" + Data.ID_Table
                    //+ "，数据库索引：" + EditData.IndexDB 
                    + "，类型：" + IDtypeName.GetCategory(Data.ID_Type)    //EditData.ID_Type  
                    + "，未知列：" + Data.ID_Unknown
                    + "，文本索引：" + Data.ID_Index + "。";
        }

        private LangSearchModel SetEditedData()
        {
            var EditedData = new LangSearchModel();

            EditedData.ID_Table = EditData.ID_Table;
            EditedData.ID_Type = EditData.ID_Type;
            EditedData.ID_Unknown = EditData.ID_Unknown;
            EditedData.ID_Index = EditData.ID_Index;
            EditedData.Text_EN = EditData.Text_EN;
            EditedData.Text_SC = textBox_ZH.Text;

            return EditedData;
        }
    }
}
