using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class LangTextArchiveRepository : BaseRepository<LangTextArchive, Guid>, ILangTextArchiveRepository
    {
        public LangTextArchiveRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
