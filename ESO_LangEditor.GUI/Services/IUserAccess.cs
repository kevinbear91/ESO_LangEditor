using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ESO_LangEditor.GUI.Services
{
    public interface IUserAccess
    {
        //Task<T> GetTokenByLogin<T>(T Dto);
        //Task<T> GetTokenByCurrentToken<T>(TokenDto Dto);
        Task<ApiMessageWithCode> AddNewRole(string roleName);
        Task<ApiMessageWithCode> AddNewUser(RegistrationUserDto registrationUserDto);
        Task<TokenDto> GetTokenByDto<T>(T Dto);
        Task<TokenDto> GetTokenByLogin(LoginUserDto loginUserDto);
        Task<List<UserInClientDto>> GetUserList();
        Task<UserDto> GetUserInfoFromServer(Guid userGuid);
        Task<string> GetUserPasswordRecoveryCode(Guid userGuid);
        Task<string> GetRegistrationCode();
        Task<List<string>> GetUserRoles(Guid userGuid);
        List<string> GetUserRoleFromToken(string token);
        Task<ApiMessageWithCode> SetUserRoles(Guid userId, List<string> roles);
        Task<ApiMessageWithCode> SetUserPasswordChange(UserPasswordChangeDto userPasswordChangeDto);
        Task<ApiMessageWithCode> SetUserPasswordByRecoveryCode(UserPasswordRecoveryDto userPasswordResetDto);
        Task<ApiMessageWithCode> SetUserPasswordToRandom(Guid userGuid);
        Task<ApiMessageWithCode> SetUserInfoChange(UserInfoChangeDto userInfoChangeDto);
        Task<ApiMessageWithCode> SetUserInfo(SetUserInfoDto setUserInfoDto);
        void SaveToken(TokenDto token);

    }
}
