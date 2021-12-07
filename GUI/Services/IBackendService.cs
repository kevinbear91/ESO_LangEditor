using Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GUI.Services
{
    public interface IBackendService
    {
        Task<AppConfigServer> GetServerRespondAndConfig();
        Task DownloadFileFromServer(string downloadPath, string localFileName, string fileSha256);
        Task<string> GetIdType(int id);
        Task<string> GetGameVersionName(int ApiId);
        bool GetFileExistAndSha256(string filePath, string fileSHA265);
        event EventHandler<string> DownloadAndExtractComplete;
    }
}
