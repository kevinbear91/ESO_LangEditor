using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public interface ILangTextRevNumberRepository : IBaseRepository<LangTextRevNumber>, IBaseRepository2<LangTextRevNumber, int>
    {

    }
}
