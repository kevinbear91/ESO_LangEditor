using ESO_LangEditorServer.Entities;
using ESO_LangEditorServer.Helpers;
using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public interface ILangTextRepository : IRepositoryBase<LangText>, IRepositoryBase2<LangText, Guid>
    {
        //Task<IEnumerable<LangText>> GetLangTextAsync(Guid userId);
        Task<IEnumerable<LangText>> GetLangTextAsync(Guid langTextId);
        Task<LangText> GetLangTextAsync(Guid userId, Guid langTextId);
        Task<PagedList<LangText>> GetAllAsync(LangTextResourceParameters parameters);
    }
}
