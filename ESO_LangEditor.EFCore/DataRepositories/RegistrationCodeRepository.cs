using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.EFCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public class RegistrationCodeRepository : BaseRepository<UserRegistrationCode, string>, IRegistrationCodeRepository
    {
        public RegistrationCodeRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
