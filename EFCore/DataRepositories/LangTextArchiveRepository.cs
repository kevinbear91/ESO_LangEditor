using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public class LangTextArchiveRepository : BaseRepository<LangTextArchive, Guid>, ILangTextArchiveRepository
    {
        public LangTextArchiveRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
