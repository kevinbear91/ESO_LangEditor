using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetUsers();
        UserDto GetUser(Guid userID);
        bool IsUserExists(Guid userID);
    }
}
