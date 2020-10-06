using ESO_LangEditorLib.Models;
using ESO_LangEditorLib.Models.Client;
using ESO_LangEditorLib.Models.Client.Enum;
using ESO_LangEditorLib.Services.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditorGUI.Services
{
    public class StartupDBCheck
    {
        private string _dbPath;
        private string _dbUpdatePath;
        private readonly LocalSearchLangText _search = new LocalSearchLangText();

        public StartupDBCheck(string dbPath, string dbUpdatePath)
        {
            _dbPath = dbPath;
            _dbUpdatePath = dbUpdatePath;
        }
        public bool CheckDbUpdateExist
        {
            get { return File.Exists(_dbUpdatePath); }
        }

        public bool IsDBExist
        {
            get { return File.Exists(_dbPath); }
        }

        public async Task<ProcessDbUpdateResult> ProcessUpdateMerge()
        {
            List<ESO_LangEditorLib.Models.Client.LangTextDto> SearchData;
            //List<LangLuaDto> searchLuaData;

            ProcessDbUpdateResult result;// = ProcessDbUpdateResult.Success;
            var exportTranslate = new LangExportFromDb();

            SearchData = await _search.GetLangTexts(SearchPostion.Full, SearchTextType.TranslateStatus, "1");
            //searchLuaData = await _search.GetLuaLangsListAsync(5, 0, "1");

            if (SearchData.Count >= 1)
            {
                string exportPath = exportTranslate.ExportTranslatedLang(SearchData);

                if (File.Exists(exportPath))
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    File.Delete(_dbPath);
                    File.Move(_dbUpdatePath, _dbPath);
                    File.Delete(_dbUpdatePath);

                    MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
                        + Environment.NewLine
                        + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
                        "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    result = ProcessDbUpdateResult.Success;
                }
                else
                {
                    result = ProcessDbUpdateResult.UnableToExportLangText;
                    MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                
            }
            else
            {
                MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                result = ProcessDbUpdateResult.UnableToFindDbFile;
            }
            return result;

            //#region  检查CSV数据库更新
            //if (File.Exists(DBPath) && File.Exists(csvDataUpdatePath))
            //{
            //    var db = new LangDbController();
            //    SearchData = await db.GetLangsListAsync(6, 0, "1");
            //    searchLuaData = await db.GetLuaLangsListAsync(5, 0, "1");

            //    if (SearchData.Count >= 1)
            //    {
            //        var exportTranslate = new ExportFromDB();
            //        string exportPath = exportTranslate.ExportTranslateDB(SearchData);

            //        if (File.Exists(exportPath))
            //        {
            //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
            //                + Environment.NewLine
            //                + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
            //                "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //        }
            //        else
            //        {
            //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }
            //    if (searchLuaData.Count >= 1)
            //    {
            //        var exportTranslate = new ExportFromDB();
            //        string exportPath = exportTranslate.ExportTranslateDB(searchLuaData);

            //        if (File.Exists(exportPath))
            //        {
            //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
            //                + Environment.NewLine
            //                + "请将 " + exportPath + " 发送给校对或导入人员，你自己也请使用导入翻译功能导入到更新的数据库！",
            //                "提示", MessageBoxButton.OK, MessageBoxImage.Information);

            //        }
            //        else
            //        {
            //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //        }
            //    }


        }

        public void RenameUpdateDB()
        {
            File.Move(_dbUpdatePath, _dbPath);
            File.Delete(_dbUpdatePath);
            //result = ProcessDbUpdateResult.Success;
        }
    }
}
