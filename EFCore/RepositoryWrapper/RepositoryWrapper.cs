using EFCore.DataRepositories;
using EFCore.DataRepositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.RepositoryWrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ILangTextRepository _langtextRepository = null;
        private readonly ILangTextReviewRepository _langtextReviewRepository = null;
        private readonly ILangTextArchiveRepository _langtextArchiveRepository = null;
        private readonly ILangTextRevisedRepository _langTextRevisedRepository = null;
        private readonly ILangTextRevNumberRepository _langTextRevNumberRepository = null;
        private readonly IRegistrationCodeRepository _registrationCodeRepository = null;
        private readonly IGameVersionRepository _gameVersionRepository = null;
        private readonly ILangTypeCatalogRepository _langTypeCatalogRepository = null;

        public LangtextApiDbContext LangtextApiDbContext { get; }

        public ILangTextRepository LangTextRepo => _langtextRepository ?? new LangTextRepository(LangtextApiDbContext);

        public ILangTextReviewRepository LangTextReviewRepo => _langtextReviewRepository ?? new LangTextReivewRepository(LangtextApiDbContext);

        public ILangTextArchiveRepository LangTextArchiveRepo => _langtextArchiveRepository ?? new LangTextArchiveRepository(LangtextApiDbContext);

        public ILangTextRevisedRepository LangTextRevisedRepo => _langTextRevisedRepository ?? new LangTextRevisedRepository(LangtextApiDbContext);

        public ILangTextRevNumberRepository LangTextRevNumberRepo => _langTextRevNumberRepository ?? new LangTextRevNumberRepository(LangtextApiDbContext);

        public IRegistrationCodeRepository RegistrationCodeRepo => _registrationCodeRepository ?? new RegistrationCodeRepository(LangtextApiDbContext);

        public IGameVersionRepository GameVersionRepo => _gameVersionRepository ?? new GameVersionRepository(LangtextApiDbContext);

        public ILangTypeCatalogRepository LangTypeCatalogRepo => _langTypeCatalogRepository ?? new LangTypeCatalogRepository(LangtextApiDbContext);

        public RepositoryWrapper(LangtextApiDbContext langtextApiDbContext)
        {
            LangtextApiDbContext = langtextApiDbContext;
        }


    }
}
