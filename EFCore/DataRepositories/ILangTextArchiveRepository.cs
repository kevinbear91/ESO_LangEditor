using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public interface ILangTextArchiveRepository : IBaseRepository<LangTextArchive>, IBaseRepository2<LangTextArchive, Guid>
    {

    }
}
