using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public interface ILangTextReviewRepository : IBaseRepository<LangTextReview>, IBaseRepository2<LangTextReview, Guid>
    {

    }
}
