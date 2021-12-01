using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.DataRepositories
{
    public interface ILangTypeCatalogReviewRepository : IBaseRepository<LangTypeCatalogReview>, IBaseRepository2<LangTypeCatalogReview, int>
    {

    }
}
