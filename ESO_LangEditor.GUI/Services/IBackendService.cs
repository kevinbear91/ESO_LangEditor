using ESO_LangEditor.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IBackendService
    {
        Task LangtextZhUpdateUpload(LangTextForUpdateZhDto langTextUpdateZhDto);
        Task LangtextZhUpdateUpload(List<LangTextForUpdateZhDto> langTextUpdateZhDto);
        void SyncToken();
        //Task StartupConnectSequenceCheck();

    }
}
