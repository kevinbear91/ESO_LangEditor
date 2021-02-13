using ESO_LangEditor.EFCore.DataRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.RepositoryWrapper
{
    public interface IRepositoryWrapperClient
    {
        ILangTextRepository LangTextRepo { get; }
        ILangTextRevNumberRepository LangTextRevNumberRepo { get; }
    }
}
