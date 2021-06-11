using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.NetClient
{
    public interface IUserAccess
    {
        Task<TokenDto> GetTokenFromServer(string url, object Dto);
        Task<List<UserInClientDto>> GetUserList(string token);
        Task<UserDto> GetUserInfoFromServer(Guid userGuid);
        Task<string> GetUserPwResetCode(Guid userGuid);
        Task<List<string>> GetUserRoles(Guid userGuid);
        Task<string> SetUserRoles(Guid userId, List<string> roles);
        Task<string> GetRegistrationCode();
    }
}
