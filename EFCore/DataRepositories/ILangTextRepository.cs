using Core.Entities;
using Core.RequestParameters;
using EFCore.Repositories;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.DataRepositories
{
    public interface ILangTextRepository : IBaseRepository<LangText>, IBaseRepository2<LangText, Guid>
    {
        Task<PagedList<LangText>> GetLangTextsByConditionAsync(Expression<Func<LangText, bool>> expression, LangTextParameters pageParameters);
        Task<PagedList<LangText>> GetLangTextsZhByConditionAsync(LangTextParameters langTextParameters);
        Task<PagedList<LangText>> GetLangTextsEnByConditionAsync(LangTextParameters langTextParameters);
    }
}
