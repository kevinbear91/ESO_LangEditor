using ESO_LangEditorServer.Data;
using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public class LangTextMockRepository : ILangTextRepository
    {
        public LangTextDto GetLangTextForUser(Guid userID, Guid langTextID)
        {
            return LangServerMockData.Current.Langs.FirstOrDefault(b => b.UserId == userID && b.Id == langTextID);
        }

        public IEnumerable<LangTextDto> GetLangTextsForUser(Guid userID)
        {
            return LangServerMockData.Current.Langs.Where(b => b.UserId == userID).ToList();
        }
    }
}
