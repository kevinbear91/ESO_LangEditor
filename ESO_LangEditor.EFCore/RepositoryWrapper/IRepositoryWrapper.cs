using ESO_LangEditor.EFCore.DataRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.RepositoryWrapper
{
    public interface IRepositoryWrapper
    {
        ILangTextRepository LangTextRepo { get; }
        ILangTextReviewRepository LangTextReviewRepo { get; }
        ILangTextArchiveRepository LangTextArchiveRepo { get; }
        ILangTextRevisedRepository LangTextRevisedRepo { get; }
        ILangTextRevNumberRepository LangTextRevNumberRepo { get; }
    }
}
