using ESO_LangEditorGUI.Model;
using ESO_LangEditorGUI.View;
using ESO_LangEditorGUI.ViewModels;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ESO_LangEditorGUI.View
{
    /// <summary>
    /// TextEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TextEditor : Window
    {
        private LangTextDto EditData;
        //private LuaUIData EditLuaData;

        private LangDbController db = new LangDbController();

        private bool _isTextZhChanged = false;

        private TextEditorViewModel _dataContext { get; }

        public DataGridViewModel LangDataGridView { get; }

        private bool isLua;

        IDCatalog IDtypeName;
        MainWindow Mainwindow;

        public TextEditor(List<LangTextDto> langData)
        {

            InitializeComponent();
            _dataContext = new TextEditorViewModel(LangDataGrid, langData, Snackbar_SaveInfo.MessageQueue);
            DataContext = _dataContext;
            this.Height = 400;

            //_dataContext.EditorList = langData;
            //_dataContext.CurrentLangText = langData.ElementAt(0);

            

            //textBox_ZH.TextChanged += new TextChangedEventHandler(TextChanged);

            Closed += TextEditorWindow_Closed;

            //EditData = LangData;
            isLua = false;

            //InitWindowVariables(false, ref IDtype, window);

        }

        public TextEditor(LangTextDto langData)
        {

            InitializeComponent();
            _dataContext = new TextEditorViewModel(LangDataGrid, langData, Snackbar_SaveInfo.MessageQueue);
            DataContext = _dataContext;
            this.Height = 400;

            //textBox_ZH.TextChanged += new TextChangedEventHandler(TextChanged);

            Closed += TextEditorWindow_Closed;

            //EditData = LangData;
            isLua = false;

            //InitWindowVariables(false, ref IDtype, window);

        }

        private void TextEditorWindow_Closed(object sender, EventArgs e)
        {
            //MainWindowViewModel viewModel = (MainWindowViewModel)DataContext;
            //viewModel.SaveSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            if(_dataContext.LangTextZh != _dataContext.CurrentLangText.TextZh)
            {
                base.OnClosing(e);
                MessageBoxResult result = MessageBox.Show("确定要关闭窗口？当前文本修改后未保存。", "关闭确认", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                e.Cancel = result == MessageBoxResult.Cancel;
            }
            else
            {
                base.OnClosing(e);
            }
            
        }

        //private void TextChanged(object Sender, TextChangedEventArgs e)
        //{
        //    if(textBox_ZH.Text != _dataContext.CurrentLangText.TextZh)
        //        _isTextZhChanged = true;
        //    else
        //        _isTextZhChanged = false;
        //}

        public void ApplyReplacedList(List<ESO_LangEditorLib.Models.Client.LangTextDto> replacedList)
        {
            //SelectedItems = replacedList;
            
            //if (replacedList.Count >= 1)
            //{
            //    ReplacedItems = replacedList;

            //    foreach (var replaced in replacedList)
            //    {
            //        foreach (var item in SelectedItems)
            //        {
            //            if (item.UniqueID == replaced.UniqueID)
            //            {
            //                item.Text_ZH = replaced.Text_ZH;
            //            }
            //        }
            //    }

            //    SaveMessagePopup("总共替换了 " + replacedList.Count + " 条文本，请查看列表确认！点击 批量保存 按钮来保存。");
            //    button_save.IsEnabled = false;
            //    InitWindowVariables(true);
            //}
            //else
            //    SaveMessagePopup("没有匹配到任何文本，请检查！");
        }






        private void Button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_save_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentLangText.TextZh = textBox_ZH.Text;
            _isTextZhChanged = false;
        }

        //private async void Button_save_Click(object sender, RoutedEventArgs e)
        //{
        //    //var EditedData = new LangSearchModel();
            
        //    //connDB.ConnectTranslateDB();


        //    if(isLua)
        //    {
        //        button_save.IsEnabled = false;

        //        var EditLuaData = GetEditedLuaData();
        //        //await Task.Run(() => db.UpdateLangsZH(EditLuaData));
        //        int result = await db.UpdateLangsZH(EditLuaData);
        //        SaveMessagePopup(result);

        //        //MessageBox.Show("ID: " + EditLuaData.UniqueID +
        //        //    Environment.NewLine + "ZH: " + EditLuaData.Text_ZH);

        //        //MessageBox.Show(updateResult);

        //        if (List_dataGrid.Items.Count > 1)
        //        {
        //            int i = List_dataGrid.SelectedIndex;

        //            //List_dataGrid.Items.RemoveAt(i);
        //            SelectedLuaItems.RemoveAt(i);

        //            List_dataGrid.ItemsSource = null;
        //            List_dataGrid.ItemsSource = SelectedLuaItems;

        //            if (i > 0)
        //            {
        //                List_dataGrid.SelectedIndex = i - 1;
        //            }
        //            else
        //            {
        //                List_dataGrid.SelectedIndex = 0;
        //            }
        //            //MessageBox.Show(List_dataGrid.SelectedIndex.ToString() + ", " + i);
        //            EditLuaData = SelectedLuaItems.ElementAt(List_dataGrid.SelectedIndex);
        //            UpdateUIContent();
        //            button_save.IsEnabled = true;
        //        }
        //        else
        //        {
        //            button_save.IsEnabled = true;
        //            this.Close();
        //        }
        //    }
        //    else
        //    {
        //        button_save.IsEnabled = false;

        //        EditData.TextZh = textBox_ZH.Text;


        //        //var EditedData = GetEditedData();
        //        int result = 1;// await db.UpdateLangsZH(EditedData);

        //        //Mainwindow.SetSaveStats(true);
        //        SaveMessagePopup(result);

        //        //save_stats.Foreground = Brushes.Green;
        //        //save_stats.Text = "保存成功！";
        //        if (List_dataGrid.Items.Count > 1)
        //        {
        //            int i = List_dataGrid.SelectedIndex;

        //            //List_dataGrid.Items.RemoveAt(i);   //当 ItemsSource 正在使用时操作无效。改用 ItemsControl.ItemsSource 访问和修改元素。
        //            SelectedItems.RemoveAt(i);

        //            List_dataGrid.ItemsSource = null;
        //            List_dataGrid.ItemsSource = SelectedItems;

        //            if (i > 0)
        //            {
        //                List_dataGrid.SelectedIndex = i - 1;
        //            }
        //            else
        //            {
        //                List_dataGrid.SelectedIndex = 0;
        //            }
        //            //MessageBox.Show(List_dataGrid.SelectedIndex.ToString() + ", " + i);
        //            EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);
        //            UpdateUIContent();
        //            button_save.IsEnabled = true;
        //        }
        //        else
        //        {
        //            button_save.IsEnabled = true;
        //            this.Close();
        //        }
        //        //MessageBox.Show(updateResult);
        //    }

        //}

        private void SaveMessagePopup(int result)
        {
            var messageQueue = Snackbar_SaveInfo.MessageQueue;
            string message;

            if (result == 1)
            {
                //if (isLua)
                //    //message = "文本ID：" + GetEditedLuaData().UniqueID + " 保存成功！";
                //else
                    message = "文本ID：" + GetEditedData().Id + " 保存成功！";
            }
            else if (result > 1)
            {
                message = result + " 条文本保存成功！";
            }
            else
            {
                message = "保存失败！";
            }

            messageQueue.Enqueue(message);
        }
        private void SaveMessagePopup(string reslut)
        {
            var messageQueue = Snackbar_SaveInfo.MessageQueue;
            //string message;

            messageQueue.Enqueue(reslut);
        }

        private void List_expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.Height = 600;
            
            //List_expander.Header = "收起列表";
        }

        private void List_expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 400;
            //List_expander.Header = "展开列表";
        }

        private void List_datagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var IDtypeName = new IDCatalog();
            DataGrid datagrid = sender as DataGrid;

            if (datagrid.SelectedIndex != -1)
            {
                if(isLua)
                {
                    //EditLuaData = SelectedLuaItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
                }
                else
                {
                    //EditData = SelectedItems.ElementAt(List_dataGrid.SelectedIndex);
                    UpdateUIContent();
                }

                
                //selectedListIndex = List_dataGrid.SelectedIndex;
            }


        }

        private void UpdateUIContent()
        {

            if(isLua)
            {
                //textBox_EN.Text = EditLuaData.Text_EN;
                //textBox_ZH.Text = EditLuaData.Text_ZH;

                //string modifyInfo = "，编辑信息：正常.";

                //if (EditLuaData.RowStats == 2 && EditLuaData.IsTranslated == 2)
                //    modifyInfo = "，编辑信息：本条内容在 " + EditLuaData.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

                ////if (EditData.RowStats == 3 && EditData.IsTranslated == 2)
                ////    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

                //textblock_information.Text = "字段：" + EditLuaData.UniqueID  + modifyInfo;
            }
            else
            {
                textBox_EN.Text = EditData.TextEn;
                textBox_ZH.Text = EditData.TextZh;

                string modifyInfo = "，编辑信息：正常.";

                if (/*EditData.RowStats == 2 &&*/ EditData.IsTranslated == 2)
                    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，可能与原文不匹配。";

                //if (EditData.RowStats == 3 && EditData.IsTranslated == 2)
                //    modifyInfo = "，编辑信息：本条内容在 " + EditData.UpdateStats + " 版本做出了修改，已经更新了对应的翻译。";

                textblock_information.Text = "类型：" + IDtypeName.GetCategory(EditData.IdType)
                        + modifyInfo;
            }
            
        }

       

        private ESO_LangEditorLib.Models.Client.LangTextDto GetEditedData()
        {
            //int rowStats = EditData.RowStats;
            //int translate = EditData.IsTranslated;

            //if (rowStats == 2)
            //    rowStats = 4;
            //else if (rowStats != 4)
            //    rowStats = 3;
            //else
            //    rowStats = 4;

            var EditedData = new ESO_LangEditorLib.Models.Client.LangTextDto
            {
                Id = EditData.Id,
                //ID = EditData.ID,
                //Unknown = EditData.Unknown,
                //Lang_Index = EditData.Lang_Index,
                //Text_EN = EditData.Text_EN,
                TextZh = textBox_ZH.Text,
                //UpdateStats = EditData.UpdateStats,
                //RowStats = rowStats,
                IsTranslated = 1,
            };

            return EditedData;
        }

        //private LuaUIData GetEditedLuaData()
        //{
        //    int rowStats = EditLuaData.RowStats;
        //    //int translate = EditData.IsTranslated;

        //    if (rowStats == 2)
        //        rowStats = 4;
        //    else if (rowStats != 4)
        //        rowStats = 3;
        //    else
        //        rowStats = 4;

        //    var EditedData = new LuaUIData
        //    {
        //        UniqueID = EditLuaData.UniqueID,
        //        //ID = EditData.ID,
        //        //Unknown = EditData.Unknown,
        //        //Lang_Index = EditData.Lang_Index,
        //        //Text_EN = EditData.Text_EN,
        //        Text_ZH = textBox_ZH.Text,
        //        //UpdateStats = EditData.UpdateStats,
        //        RowStats = rowStats,
        //        IsTranslated = 1,
        //    };

        //    return EditedData;
        //}

        private void Button_modifyList_Click(object sender, RoutedEventArgs e)
        {
            //var modifyListWindow = new TextEditor_SearchReplace(SelectedItems, this);

            //modifyListWindow.Show();
        }

        private async void Button_saveModifyList_Click(object sender, RoutedEventArgs e)
        {
            //int result = await db.UpdateLangsZH(ReplacedItems);

            //if (result >= 1)
            //{
            //    CompareReplacedItemInList();

            //    Debug.WriteLine(SelectedItems.Count);

            //    SaveMessagePopup(result);
            //    InitWindowVariables(false);

            //    button_saveModifyList.IsEnabled = false;
            //}

            //button_save.IsEnabled = true;
        }

        private void CompareReplacedItemInList()
        {
            //Dictionary<string, LangText> selectedItem = new Dictionary<string, LangText>();
            //Dictionary<string, LangText> replacedItem = new Dictionary<string, LangText>();

            //selectedItem = SelectedItems.ToDictionary(s => s.UniqueID);
            //replacedItem = ReplacedItems.ToDictionary(r => r.UniqueID);

            //foreach (var item in replacedItem)
            //{
            //    if (selectedItem.Keys.Contains(item.Key))
            //        selectedItem.Remove(item.Key);
            //}

            //SelectedItems = selectedItem.Values.ToList();
        }
    }
}
