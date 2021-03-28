using ESO_LangEditor.EFCore.DataRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.EFCore.RepositoryWrapper
{
    public class RepositoryWrapperClient : IRepositoryWrapperClient
    {
        private readonly ILangTextRepository _langtextRepository = null;
        private readonly ILangTextRevNumberRepository _langTextRevNumberRepository = null;
        private readonly IUserInClientRepository _userInClientRepository = null;

        public LangtextClientDbContext LangtextClientDbContext { get; }

        public ILangTextRevNumberRepository LangTextRevNumberRepo => _langTextRevNumberRepository ?? new LangTextRevNumberRepository(LangtextClientDbContext);

        public ILangTextRepository LangTextRepo => _langtextRepository ?? new LangTextRepositoryClient(LangtextClientDbContext);

        public IUserInClientRepository UserInClientRePo => _userInClientRepository ?? new UserInClientRepository(LangtextClientDbContext);

        public RepositoryWrapperClient(LangtextClientDbContext langtextClientDbContext)
        {
            LangtextClientDbContext = langtextClientDbContext;
        }
    }
}
