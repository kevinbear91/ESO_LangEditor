using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class LangTextRevNumberRepository : BaseRepository<LangTextRevNumber, int>, ILangTextRevNumberRepository
    {
        public LangTextRevNumberRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
