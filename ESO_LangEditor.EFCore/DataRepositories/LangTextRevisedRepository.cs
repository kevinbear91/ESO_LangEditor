using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class LangTextRevisedRepository : BaseRepository<LangTextRevised, int>, ILangTextRevisedRepository
    {
        public LangTextRevisedRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
