using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.RequestParameters;
using ESO_LangEditor.Core.RequestParameters.Extensions;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class LangTextRepository : BaseRepository<LangText, Guid>, ILangTextRepository
    {
        public LangTextRepository(DbContext dbcontext) : base(dbcontext)
        {

        }

        public async Task<PagedList<LangText>> GetLangTextsByConditionAsync(Expression<Func<LangText, bool>> expression, LangTextParameters pageParameters)
        {
            var items = await DbContext.Set<LangText>()
                .Where(expression).FilterLangTexts(pageParameters)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .ToListAsync();

            var count = await DbContext.Set<LangText>().CountAsync();

            return new PagedList<LangText>(items, count, pageParameters.PageNumber, pageParameters.PageSize);
        }

        public async Task<PagedList<LangText>> GetLangTextsZhByConditionAsync(LangTextParameters langTextParameters, string searchTerm)
        {
            var items = await DbContext.Set<LangText>()
                .SearchLangTextsZh(langTextParameters, searchTerm)
                .FilterLangTexts(langTextParameters)
                .ToListAsync();

            return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        }

        public async Task<PagedList<LangText>> GetLangTextsEnByConditionAsync(LangTextParameters langTextParameters, string searchTerm)
        {
            var items = await DbContext.Set<LangText>()
                .SearchLangTextsEn(langTextParameters, searchTerm)
                .FilterLangTexts(langTextParameters)
                .ToListAsync();

            return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        }
    }
}
