using ESO_LangEditorGUI.View;
using ESO_LangEditorLib;
using ESO_LangEditorLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace ESO_LangEditorGUI.Controller
{
    public class CheckDBFile
    {
        public async void CheckDBUpdateFile(MainWindow window)
        {
            string csvDataUpdatePath = @"Data\LangData.update";
            string DBPath = @"Data\LangData.db";

            List<LangText> SearchData;
            List<LuaUIData> searchLuaData;


            #region  检查CSV数据库更新
            if (File.Exists(DBPath) && File.Exists(csvDataUpdatePath))
            {
                var db = new LangDbController();
                SearchData = await db.GetLangsListAsync(6, 0, "1");
                searchLuaData = await db.GetLuaLangsListAsync(5, 0, "1");

                if (SearchData.Count >= 1)
                {
                    var exportTranslate = new ExportFromDB();
                    string exportPath = exportTranslate.ExportTranslateDB(SearchData);

                    if (File.Exists(exportPath))
                    {
                        MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
                            + Environment.NewLine
                            + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
                            "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                if (searchLuaData.Count >= 1)
                {
                    var exportTranslate = new ExportFromDB();
                    string exportPath = exportTranslate.ExportTranslateDB(searchLuaData);

                    if (File.Exists(exportPath))
                    {
                        MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
                            + Environment.NewLine
                            + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
                            "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
                File.Delete(DBPath);
                File.Move(csvDataUpdatePath, DBPath);
                File.Delete(csvDataUpdatePath);

                SetMainWindowButton(window,true);

            }
            else if (File.Exists(DBPath))
            {
                SetMainWindowButton(window, true);
            }
            else if (File.Exists(csvDataUpdatePath))
            {
                File.Move(csvDataUpdatePath, DBPath);
                File.Delete(csvDataUpdatePath);

                SetMainWindowButton(window, true);
            }
            else
            {
                MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                SetMainWindowButton(window, false);
            }

            static void SetMainWindowButton(MainWindow window, bool isEnable)
            {
                window.SearchTextBox.IsEnabled = isEnable;
                window.SearchButton.IsEnabled = isEnable;
            }
            #endregion
        }
    }
}
