using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient.Old
{
    public interface IUserAccess
    {
        Task Get(Guid userId);
        Task GetToken(Guid userId, object tokenDto);
    }
}
