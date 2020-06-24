using ESO_Lang_Editor.Model;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ESO_Lang_Editor.View
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        private LangText EditData;
        private List<LangText> SelectedItems;

        private LuaUIData EditLuaData;
        private List<LuaUIData> SelectedLuaItems;

        private LangDbController db = new LangDbController();
        private bool isLua;

        IDCatalog IDtypeName;
        MainWindow Mainwindow;

        public TextEditor(LangText LangData, ref IDCatalog IDtype, MainWindow window)
        {

            InitializeComponent();
            Mainwindow = window;

            save_stats.Text = "";

            this.Height = 400;
            IDtypeName = IDtype;

            EditData = LangData;

            isLua = false;

            GeneratingColumns();
            GridMover.Visibility = Visibility.Collapsed;
            List_expander.Visibility = Visibility.Collapsed;

            textBox_EN.IsReadOnly = true;
            UpdateUIContent();
            //List_Datagrid_Display(LangData);

        }

        public TextEditor(LuaUIData LangLuaData, ref IDCatalog IDtype, MainWindow window)
        {

            InitializeComponent();
            Mainwindow = window;
            save_stats.Text = "";

            this.Height = 400;
            IDtypeName = IDtype;

            EditLuaData = LangLuaData;

            isLua = true;

            GeneratingColumns();
            GridMover.Visibility = Visibility.Collapsed;
            List_expander.Visibility = Visibility.Collapsed;

            textBox_EN.IsReadOnly = true;
            UpdateUIContent();
            //List_Datagrid_Display(LangData);

        }

        public TextEditor(List<LangText> SelectedItemsList, ref IDCatalog IDtype, MainWindow window)
        {

            InitializeComponent();
            Mainwindow = window;
            save_stats.Text = "";

            this.Height = 600;
            IDtypeName = IDtype;

            isLua = false;
            SelectedItems = SelectedItemsList;
            GeneratingColumns();
            textBox_EN.IsReadOnly = true;

            List_Datagrid_Display();


        }

        public TextEditor(List<LuaUIData> SelectedLuaItemsList, ref IDCatalog IDtype, MainWindow window)
        {

            InitializeComponent();
            Mainwindow = window;
            save_stats.Text = "";

            this.Height = 600;
            IDtypeName = IDtype;

            isLua = true;
            SelectedLuaItems = SelectedLuaItemsList;
            GeneratingColumns();
            textBox_EN.IsReadOnly = true;

            List_Datagrid_Display();

        }


        private void GeneratingColumns()
        {
            if (isLua)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "UI ID";
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

        private async void button_save_Click(object sender, RoutedEventArgs e)
        {
            //var EditedData = new LangSearchModel();
            
            //connDB.ConnectTranslateDB();


            if(isLua)
            {
                button_save.IsEnabled = false;

                var EditLuaData = GetEditedLuaData();
                await Task.Run(() => db.UpdateLangsZH(EditLuaData));



                MessageBox.Show("ID: " + EditLuaData.UniqueID +
                    Environment.NewLine + "ZH: " + EditLuaData.Text_ZH);

                //MessageBox.Show(updateResult);

                if (List_dataGrid.Items.Count > 1)
                {
                    int i = List_dataGrid.SelectedIndex;

                    List_dataGrid.Items.RemoveAt(i);
                    SelectedLuaItems.RemoveAt(i);

                    if (i > 0)
                    {
                        List_dataGrid.SelectedIndex = i - 1;
                    }
                    else
                    {
                        List_dataGrid.SelectedIndex = 0;
                    }
                    //MessageBox.Show(List_dataGrid.SelectedIndex.ToString() + ", " + i);
                    EditLuaData = SelectedLuaItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
                    button_save.IsEnabled = true;
                }
                else
                {
                    button_save.IsEnabled = true;
                    this.Close();
                }
            }
            else
            {
                button_save.IsEnabled = false;

                var EditedData = GetEditedData();
                int result = await db.UpdateLangsZH(EditedData);

                if(result != 0)
                {
                    Mainwindow.SetSaveStats(true);

                    save_stats.Foreground = Brushes.Green;
                    save_stats.Text = "保存成功！";
                    if (List_dataGrid.Items.Count > 1)
                    {
                        int i = List_dataGrid.SelectedIndex;

                        //List_dataGrid.Items.RemoveAt(i);   //当 ItemsSource 正在使用时操作无效。改用 ItemsControl.ItemsSource 访问和修改元素。
                        SelectedItems.RemoveAt(i);

                        List_dataGrid.ItemsSource = null;

                        List_dataGrid.ItemsSource = SelectedItems;



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
                        button_save.IsEnabled = true;
                    }
                    else
                    {
                        button_save.IsEnabled = true;
                        this.Close();
                    }

                    await Task.Delay(2000);
                    save_stats.Text = "";
                }
                else
                {
                    save_stats.Foreground = Brushes.Red;
                    save_stats.Text = "保存失败！";
                }

                //MessageBox.Show(updateResult);

                
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

            if (isLua)
            {
                if (SelectedLuaItems != null && SelectedLuaItems.Count > 1)
                {
                    List_expander.Visibility = Visibility.Visible;
                    List_expander.IsExpanded = true;

                    List_dataGrid.ItemsSource = SelectedLuaItems;

                    //foreach (var item in SelectedItems)
                    //{
                    //    List_dataGrid.Items.Add(item);
                    //}

                    List_dataGrid.SelectedIndex = 0;


                    EditLuaData = SelectedLuaItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();

                }
                else
                {
                    List_expander.Visibility = Visibility.Hidden;
                    List_expander.IsExpanded = false;
                    //EditData = LangData;
                    UpdateUIContent();
                }
            }
            else
            {
                if (SelectedItems != null && SelectedItems.Count > 1)
                {
                    List_expander.Visibility = Visibility.Visible;
                    List_expander.IsExpanded = true;

                    List_dataGrid.ItemsSource = SelectedItems;

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
            }

        }



        private void List_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var IDtypeName = new IDCatalog();
            DataGrid datagrid = sender as DataGrid;

            if (datagrid.SelectedIndex != -1)
            {
                if(isLua)
                {
                    EditLuaData = SelectedLuaItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
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

            if(isLua)
            {
                textBox_EN.Text = EditLuaData.Text_EN;
                textBox_ZH.Text = EditLuaData.Text_ZH;

                string modifyInfo = "，编辑信息：正常.";

                if (EditLuaData.RowStats == 2 && EditLuaData.IsTranslated == 2)
                    modifyInfo = "，编辑信息：本条内容在 " + EditLuaData.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

                //if (EditData.RowStats == 3 && EditData.IsTranslated == 2)
                //    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

                textblock_information.Text = "字段：" + EditLuaData.UniqueID + "\n"  + modifyInfo;
            }
            else
            {
                textBox_EN.Text = EditData.Text_EN;
                textBox_ZH.Text = EditData.Text_ZH;

                string modifyInfo = "，编辑信息：正常.";

                if (EditData.RowStats == 2 && EditData.IsTranslated == 2)
                    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

                //if (EditData.RowStats == 3 && EditData.IsTranslated == 2)
                //    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

                textblock_information.Text = "类型：" + IDtypeName.GetCategory(EditData.ID)
                        + modifyInfo;
            }
            
        }

       

        private LangText GetEditedData()
        {
            int rowStats = EditData.RowStats;
            //int translate = EditData.IsTranslated;

            if (rowStats == 2)
                rowStats = 4;
            else if (rowStats != 4)
                rowStats = 3;
            else
                rowStats = 4;

            var EditedData = new LangText
            {
                UniqueID = EditData.UniqueID,
                //ID = EditData.ID,
                //Unknown = EditData.Unknown,
                //Lang_Index = EditData.Lang_Index,
                //Text_EN = EditData.Text_EN,
                Text_ZH = textBox_ZH.Text,
                //UpdateStats = EditData.UpdateStats,
                RowStats = rowStats,
                IsTranslated = 1,
            };

            return EditedData;
        }

        private LuaUIData GetEditedLuaData()
        {
            int rowStats = EditLuaData.RowStats;
            //int translate = EditData.IsTranslated;

            if (rowStats == 2)
                rowStats = 4;
            else if (rowStats != 4)
                rowStats = 3;
            else
                rowStats = 4;

            var EditedData = new LuaUIData
            {
                UniqueID = EditLuaData.UniqueID,
                //ID = EditData.ID,
                //Unknown = EditData.Unknown,
                //Lang_Index = EditData.Lang_Index,
                //Text_EN = EditData.Text_EN,
                Text_ZH = textBox_ZH.Text,
                //UpdateStats = EditData.UpdateStats,
                RowStats = rowStats,
                IsTranslated = 1,
            };

            return EditedData;
        }
    }
}
