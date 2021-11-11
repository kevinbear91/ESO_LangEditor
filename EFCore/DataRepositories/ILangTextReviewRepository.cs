using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public interface ILangTextReviewRepository : IBaseRepository<LangTextReview>, IBaseRepository2<LangTextReview, Guid>
    {

    }
}
