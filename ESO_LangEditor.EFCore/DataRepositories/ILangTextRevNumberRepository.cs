using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public interface ILangTextRevNumberRepository : IBaseRepository<LangTextRevNumber>, IBaseRepository2<LangTextRevNumber, int>
    {

    }
}
