using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IBackendService
    {
        Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto);
        Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDto);
        Task SyncToken();
        Task SyncUser();
        Task ExportLangFile();
        //Task StartupConnectSequenceCheck();

    }
}
