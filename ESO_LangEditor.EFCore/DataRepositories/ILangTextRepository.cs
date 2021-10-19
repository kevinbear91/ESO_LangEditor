using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.Core.RequestParameters;
using ESO_LangEditor.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public interface ILangTextRepository : IBaseRepository<LangText>, IBaseRepository2<LangText, Guid>
    {
        Task<PagedList<LangText>> GetLangTextsByConditionAsync(Expression<Func<LangText, bool>> expression, LangTextParameters pageParameters);
        Task<PagedList<LangText>> GetLangTextsZhByConditionAsync(LangTextParameters langTextParameters);
        Task<PagedList<LangText>> GetLangTextsEnByConditionAsync(LangTextParameters langTextParameters);
    }
}
