using System.IO;

namespace ESO_LangEditor.GUI.Services
{
    public class StartupDBCheck
    {
        private string _dbPath;
        private string _dbUpdatePath;
        //private readonly LangTextRepoClientService _search = new LangTextRepoClientService();

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

        //public async Task<ProcessDbUpdateResult> ProcessUpdateMerge()
        //{
        //    List<LangTextDto> SearchData;
        //    //List<LangLuaDto> searchLuaData;

        //    ProcessDbUpdateResult result;// = ProcessDbUpdateResult.Success;
        //    var exportTranslate = new ExportDbToFile();

        //    //SearchData = await _search.GetLangTextByConditionAsync("1", SearchTextType.TranslateStatus, SearchPostion.Full);
        //    ////searchLuaData = await _search.GetLuaLangsListAsync(5, 0, "1");

        //    //if (File.Exists(_dbPath))
        //    //{
        //    //    if (SearchData.Count >= 1)
        //    //    {
        //    //        string exportPath = exportTranslate.ExportLangTextsAsJson(SearchData, LangChangeType.ChangedZH);

        //    //        if (File.Exists(exportPath))
        //    //        {
        //    //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，现已将翻译过的文本导出。"
        //    //                + Environment.NewLine
        //    //                + "请将 " + exportPath + " 发送给校对或导入人员！",
        //    //                "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //    //        }
        //    //        else
        //    //        {
        //    //            MessageBox.Show("发现了新版数据库，但你本地已查询到翻译过但未导出的文本，但导出失败！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //            return ProcessDbUpdateResult.UnableToExportLangText;
        //    //        }
        //    //    }

        //    //    GC.Collect();
        //    //    GC.WaitForPendingFinalizers();
        //    //    File.Delete(_dbPath);
        //    //    File.Move(_dbUpdatePath, _dbPath);
        //    //    File.Delete(_dbUpdatePath);

        //    //    return ProcessDbUpdateResult.Success;
        //    //}
        //    //else
        //    //{
        //    //    MessageBox.Show("无法找到数据库文件！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        //    //    return ProcessDbUpdateResult.UnableToFindDbFile;
        //    //}

        //}

        public void RenameUpdateDB()
        {
            File.Move(_dbUpdatePath, _dbPath);
            File.Delete(_dbUpdatePath);
            //result = ProcessDbUpdateResult.Success;
        }
    }
}
