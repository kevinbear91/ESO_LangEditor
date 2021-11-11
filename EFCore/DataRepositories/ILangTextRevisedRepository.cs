using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public interface ILangTextRevisedRepository : IBaseRepository<LangTextRevised>, IBaseRepository2<LangTextRevised, int>
    {
    }
}
