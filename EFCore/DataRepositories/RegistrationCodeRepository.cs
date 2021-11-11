using Core.Entities;
using EFCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EFCore.DataRepositories
{
    public class RegistrationCodeRepository : BaseRepository<UserRegistrationCode, string>, IRegistrationCodeRepository
    {
        public RegistrationCodeRepository(DbContext dbcontext) : base(dbcontext)
        {

        }
    }
}
