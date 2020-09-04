using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public interface ILangTextRepository
    {
        IEnumerable<LangTextDto> GetLangTextsForUser(Guid userGuid);
        LangTextDto GetLangTextForUser(Guid userGuid, Guid langGuid);
    }
}
