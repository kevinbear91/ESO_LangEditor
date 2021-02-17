using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class LangTextRepositoryClient : BaseRepository<LangText, Guid>, ILangTextRepository
    {
        public LangTextRepositoryClient(DbContext dbcontext) : base(dbcontext)
        {
            
        }
    }
}
