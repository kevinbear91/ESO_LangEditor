using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IStartupCheck
    {
        void UpdateEditor();
        Task UpdateUpdater();
        //bool CompareDatabaseRevNumber();
        Task DownloadFullDatabase();
        Task SyncRevDatabase();
        Task StartupTaskList();
        Task Login();
        Task LoginTaskList();
        Task SyncUsers();

        //Task SyncUserInfo();
    }
}
