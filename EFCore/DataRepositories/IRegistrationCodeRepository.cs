using Core.Entities;
using EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.DataRepositories
{
    public interface IRegistrationCodeRepository : IBaseRepository<UserRegistrationCode>, IBaseRepository2<UserRegistrationCode, string>
    {

    }
}
