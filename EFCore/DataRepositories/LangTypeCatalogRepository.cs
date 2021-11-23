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
    public class LangTypeCatalogRepository : BaseRepository<LangTypeCatalog, int>, ILangTypeCatalogRepository
    {
        public LangTypeCatalogRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
