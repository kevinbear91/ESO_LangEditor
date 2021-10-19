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

        public async Task<PagedList<LangText>> GetLangTextsByConditionAsync(Expression<Func<LangText, bool>> expression, LangTextParameters langTextParameters)
        {
            var items = await DbContext.Set<LangText>()
                .Where(expression).AsNoTracking()
                .FilterLangTexts(langTextParameters)
                .ToListAsync();

            //var count = await DbContext.Set<LangText>().CountAsync();

            return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        }

        public async Task<PagedList<LangText>> GetLangTextsZhByConditionAsync(LangTextParameters langTextParameters)
        {
            var items = await DbContext.Set<LangText>()
                .SearchLangTextsZh(langTextParameters).AsNoTracking()
                .FilterLangTexts(langTextParameters)
                .ToListAsync();

            return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        }

        public async Task<PagedList<LangText>> GetLangTextsEnByConditionAsync(LangTextParameters langTextParameters)
        {
            var items = await DbContext.Set<LangText>()
                .SearchLangTextsEn(langTextParameters).AsNoTracking()
                .FilterLangTexts(langTextParameters)
                .ToListAsync();

            return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        }

        //public async Task<PagedList<LangText>> GetLangTextsByIdTypeAsync(LangTextParameters langTextParameters)
        //{
        //    var items = await DbContext.Set<LangText>()
        //        .Where(lang => lang.IdType == ToInt32(langTextParameters.SearchTerm)).AsNoTracking()
        //        .FilterLangTexts(langTextParameters)
        //        .ToListAsync();

        //    return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        //}

        //public async Task<PagedList<LangText>> GetLangTextsByGameVersionAsync(LangTextParameters langTextParameters)
        //{
        //    var items = await DbContext.Set<LangText>()
        //        .Where(lang => lang.UpdateStats == langTextParameters.SearchTerm).AsNoTracking()
        //        .FilterLangTexts(langTextParameters)
        //        .ToListAsync();

        //    return PagedList<LangText>.ToPageList(items, langTextParameters.PageNumber, langTextParameters.PageSize);
        //}
    }
}
