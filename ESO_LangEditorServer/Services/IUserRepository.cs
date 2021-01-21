using ESO_LangEditorServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditorServer.Services
{
    public interface IUserRepository
    {
        User GetUser(Guid id);
        void Save(User user);
    }
}
