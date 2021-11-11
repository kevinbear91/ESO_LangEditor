using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public class LangTextRevNumberRepository : BaseRepository<LangTextRevNumber, int>, ILangTextRevNumberRepository
    {
        public LangTextRevNumberRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
