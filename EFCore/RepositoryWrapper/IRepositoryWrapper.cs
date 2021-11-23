using EFCore.DataRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.RepositoryWrapper
{
    public interface IRepositoryWrapper
    {
        ILangTextRepository LangTextRepo { get; }
        ILangTextReviewRepository LangTextReviewRepo { get; }
        ILangTextArchiveRepository LangTextArchiveRepo { get; }
        ILangTextRevisedRepository LangTextRevisedRepo { get; }
        ILangTextRevNumberRepository LangTextRevNumberRepo { get; }
        IRegistrationCodeRepository RegistrationCodeRepo { get; }
        IGameVersionRepository GameVersionRepo { get; }
        ILangTypeCatalogRepository LangTypeCatalogRepo { get; } 
    }
}
