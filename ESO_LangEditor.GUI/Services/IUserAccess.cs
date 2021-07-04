using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IUserAccess
    {
        //Task<T> GetTokenByLogin<T>(T Dto);
        //Task<T> GetTokenByCurrentToken<T>(TokenDto Dto);
        Task<TokenDto> GetTokenByDto<T>(T Dto);
        Task<List<UserInClientDto>> GetUserList(string token);
        Task<UserDto> GetUserInfoFromServer(Guid userGuid);
        Task<string> GetUserPwResetCode(Guid userGuid);
        Task<List<string>> GetUserRoles(Guid userGuid);
        Task SetUserRoles(Guid userId, List<string> roles);
        Task<string> GetRegistrationCode();
    }
}
