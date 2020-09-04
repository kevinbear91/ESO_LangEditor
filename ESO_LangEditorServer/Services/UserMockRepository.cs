using ESO_LangEditorServer.Data;
using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public class UserMockRepository : IUserRepository
    {
        public UserDto GetUser(Guid userID)
        {
            var user = LangServerMockData.Current.Users.FirstOrDefault(ur => ur.Id == userID);
            return user;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            return LangServerMockData.Current.Users;
        }

        public bool IsUserExists(Guid userID)
        {
            return LangServerMockData.Current.Users.Any(ur => ur.Id == userID);
        }
    }
}
