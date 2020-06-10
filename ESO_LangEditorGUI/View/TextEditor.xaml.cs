using ESO_Lang_Editor.Model;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        private LangData EditData;
        private List<LangData> SelectedItems;

        //private UIstrFile EditStrData;
        //private List<UIstrFile> SelectedStrItems;

        private int selectedListIndex;

        private bool isStr;

        IDCatalog IDtypeName = new IDCatalog();

        public TextEditor(LangData LangData)
        {

            InitializeComponent();
            this.Height = 480;

            EditData = LangData;

            isStr = false;

            GeneratingColumns(isStr);
            textBox_EN.IsReadOnly = true;
            UpdateUIContent();
            //List_Datagrid_Display(LangData);

        }
        public TextEditor(List<LangData> SelectedItemsList)
        {

            InitializeComponent();
            this.Height = 600;
            isStr = false;
            SelectedItems = SelectedItemsList;
            GeneratingColumns(isStr);
            textBox_EN.IsReadOnly = true;

            List_Datagrid_Display();

        }
        //public TextEditor(UIstrFile LangData, List<UIstrFile> SelectedItemsList)
        //{

        //    InitializeComponent();
        //    this.Width = 530;

        //    isStr = true;
        //    SelectedStrItems = SelectedItemsList;
        //    GeneratingColumns(isStr);
        //    textBox_EN.IsReadOnly = false;

        //    List_Datagrid_Display(LangData);

        //}

        private void GeneratingColumns(bool isStr)
        {
            if (isStr)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "UI ID";
                c1.Binding = new Binding("UI_ID");
                //c1.Width = 100;
                List_dataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                //c2.Width = 100;
                c2.Binding = new Binding("UI_EN");
                List_dataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                //c3.Width = 100;
                c3.Binding = new Binding("UI_ZH");
                List_dataGrid.Columns.Add(c3);
            }
            else
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "文本ID";
                c1.Binding = new Binding("UniqueID");
                //c1.Width = 100;
                List_dataGrid.Columns.Add(c1);

                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "英文";
                //c2.Width = 100;
                c2.Binding = new Binding("Text_EN");
                List_dataGrid.Columns.Add(c2);

                DataGridTextColumn c3 = new DataGridTextColumn();
                c3.Header = "汉化";
                //c3.Width = 100;
                c3.Binding = new Binding("Text_ZH");
                List_dataGrid.Columns.Add(c3);
            }

        }




        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            //var EditedData = new LangSearchModel();
            
            //connDB.ConnectTranslateDB();


            if(isStr)
            {
                //var StrDB = new UI_StrController();
                //var EditedStrData = SetEditedStrData();

                //var updateResult = StrDB.UpdateStrFromEditor(EditedStrData);

                //MessageBox.Show(updateResult);

                //if (List_dataGrid.Items.Count > 1)
                //{
                //    int i = List_dataGrid.SelectedIndex;

                //    List_dataGrid.Items.RemoveAt(i);
                //    SelectedStrItems.RemoveAt(i);

                //    if (i > 0)
                //    {
                //        List_dataGrid.SelectedIndex = i - 1;
                //    }
                //    else
                //    {
                //        List_dataGrid.SelectedIndex = 0;
                //    }
                //    //MessageBox.Show(List_dataGrid.SelectedIndex.ToString() + ", " + i);
                //    SetEditDataTextBlocks(SelectedStrItems.ElementAt(List_dataGrid.SelectedIndex));
                //}
                //else
                //{
                //    this.Close();
                //}
            }
            else
            {
                var EditedData = GetEditedData();

                MessageBox.Show("ID: " + EditedData.UniqueID +
                    Environment.NewLine + "ZH: " + EditedData.Text_ZH);

                //MessageBox.Show(updateResult);

                if (List_dataGrid.Items.Count > 1)
                {
                    int i = List_dataGrid.SelectedIndex;

                    List_dataGrid.Items.RemoveAt(i);
                    SelectedItems.RemoveAt(i);

                    if (i > 0)
                    {
                        List_dataGrid.SelectedIndex = i - 1;
                    }
                    else
                    {
                        List_dataGrid.SelectedIndex = 0;
                    }
                    //MessageBox.Show(List_dataGrid.SelectedIndex.ToString() + ", " + i);
                    EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
                }
                else
                {
                    this.Close();
                }
            }


            
        }

        private void List_expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height = 600;
            
            List_expander.Header = "收起列表";
        }

        private void List_expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 480;
            List_expander.Header = "展开列表";
        }

        private void List_Datagrid_Display()
        {
            //var IDtypeName = new IDCatalog();

            if (List_dataGrid.Items.Count > 1)
                List_dataGrid.Items.Clear();

            if (SelectedItems != null && SelectedItems.Count > 1)
            {
                List_expander.Visibility = Visibility.Visible;
                List_expander.IsExpanded = true;

                List_dataGrid.ItemsSource = SelectedItems;

                //foreach (var item in SelectedItems)
                //{
                //    List_dataGrid.Items.Add(item);
                //}

                List_dataGrid.SelectedIndex = 0;


                EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);
                UpdateUIContent();

            }
            else
            {
                List_expander.Visibility = Visibility.Hidden;
                List_expander.IsExpanded = false;
                //EditData = LangData;
                UpdateUIContent();
            }
            

            //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        }


        //private void List_Datagrid_Display(UIstrFile LangData)
        //{
        //    //var IDtypeName = new IDCatalog();

        //    if (List_dataGrid.Items.Count > 1)
        //        //SearchData = null;
        //        List_dataGrid.Items.Clear();

            //SearchData = SearchLang(SearchCheck());
            //if (SelectedStrItems != null && SelectedStrItems.Count > 1)
            //{
            //    List_expander.Visibility = Visibility.Visible;
            //    List_expander.IsExpanded = true;

            //    foreach (var item in SelectedStrItems)
            //    {
            //        List_dataGrid.Items.Add(item);
            //    }
            //    List_dataGrid.SelectedIndex = 0;

            //    SetEditDataTextBlocks(SelectedStrItems.ElementAt(List_dataGrid.SelectedIndex));

            //}
            //else
            //{
            //    List_expander.Visibility = Visibility.Hidden;
            //    List_expander.IsExpanded = false;
            //    //EditData = LangData;
            //    SetEditDataTextBlocks(LangData);


            //}


            //textBlock_Info.Text = "总计搜索到" + LangSearch.Items.Count + "条结果。";
        //}


        private void List_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var IDtypeName = new IDCatalog();
            DataGrid datagrid = sender as DataGrid;

            if (datagrid.SelectedIndex != -1)
            {
                if(isStr)
                {
                   // SetEditDataTextBlocks(SelectedStrItems.ElementAt(List_dataGrid.SelectedIndex));
                }
                else
                {
                    EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
                }

                
                //selectedListIndex = List_dataGrid.SelectedIndex;
            }


        }

        private void UpdateUIContent()
        {
            textBox_EN.Text = EditData.Text_EN;
            textBox_ZH.Text = EditData.Text_ZH;




            string modifyInfo = "，编辑信息：正常.";

            if (EditData.RowStats == 2 && EditData.IsTranslated == 2)
                modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

            //if (EditData.RowStats == 3 && EditData.IsTranslated == 2)
            //    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

            textblock_information.Text = "类型：" + IDtypeName.GetCategory(EditData.ID.ToString())
                    + modifyInfo;
        }

        //private void SetEditDataTextBlocks(UIstrFile Data)
        //{
        //    string modifyInfo = "，编辑信息：正常.";

        //    if (Data.isTranslated == 2 && Data.RowStats == 20)
        //        modifyInfo = "，编辑信息：本条内容在 " + Data.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

        //    if (Data.isTranslated == 3 && Data.RowStats == 20)
        //        modifyInfo = "，编辑信息：本条内容在 " + Data.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

        //    if (Data.RowStats == 30)
        //        modifyInfo = "，编辑信息：本条内容在 " + Data.UpdateStats + " 版本删除，请勿编辑。";


        //    EditStrData = Data;

        //    textBox_EN.Text = Data.UI_EN;
        //    textBox_ZH.Text = Data.UI_ZH;
        //    textblock_information.Text = "当前表：" + Data.UI_Table
        //                                 //+ "，数据库索引：" + EditData.IndexDB 
                      
        //            //"，未知列：" + Data.ID_Unknown
        //            //+ "，文本索引：" + Data.ID_Index
        //            + modifyInfo;
        //}

        private LangData GetEditedData()
        {
            var EditedData = new LangData
            {
                UniqueID = EditData.UniqueID,
                Text_ZH = textBox_ZH.Text,
                IsTranslated = 1
            };

            return EditedData;
        }

        //private UIstrFile SetEditedStrData()
        //{
        //    var EditedData = new UIstrFile();

        //    EditedData.UI_Table = EditStrData.UI_Table;
        //    EditedData.UI_ID = EditStrData.UI_ID;
        //    EditedData.UI_EN = textBox_EN.Text;
        //    EditedData.UI_ZH = textBox_ZH.Text;
        //    EditedData.isTranslated = 1;

        //    System.Console.WriteLine("UI_Table：{0}, UI_ID：{1}, UI_EN：{2}。", EditedData.UI_Table, EditedData.UI_ID, EditedData.UI_EN);

        //    return EditedData;
        //}
    }
}
