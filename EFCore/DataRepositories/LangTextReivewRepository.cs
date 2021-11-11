using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public class LangTextReivewRepository : BaseRepository<LangTextReview, Guid>, ILangTextReviewRepository
    {
        public LangTextReivewRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
