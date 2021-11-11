using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public class LangTextRevisedRepository : BaseRepository<LangTextRevised, int>, ILangTextRevisedRepository
    {
        public LangTextRevisedRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
