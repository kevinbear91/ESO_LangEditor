using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.DataRepositories
{
    public class LangTypeCatalogReviewRepository : BaseRepository<LangTypeCatalogReview, int>, ILangTypeCatalogReviewRepository
    {
        public LangTypeCatalogReviewRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
