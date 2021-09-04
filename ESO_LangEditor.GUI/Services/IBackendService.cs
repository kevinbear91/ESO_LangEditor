using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IBackendService
    {
        //Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto);
        //Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDto);
        //void SyncToken();
        //Task StartupConnectSequenceCheck();
        Task<AppConfigServer> GetServerRespondAndConfig();
        Task DownloadFileFromServer(string downloadPath, string localFileName, string fileSha256);
        bool GetFileExistAndSha256(string filePath, string fileSHA265);
        //delegate void SetAppConfigClientUpdaterSha256(string sha256);
        //Task TestEvent();
        event EventHandler<string> SetSha256;
        //event EventHandler<string> SetAppConfigClientJpLangSha256;
    }
}
