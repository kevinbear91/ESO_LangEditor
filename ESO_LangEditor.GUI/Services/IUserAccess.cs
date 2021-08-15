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
        Task<MessageWithCode> AddNewRole(string roleName);
        Task<MessageWithCode> AddNewUser(RegistrationUserDto registrationUserDto);
        Task<TokenDto> GetTokenByTokenDto(Guid userGuid, TokenDto Dto);
        Task<TokenDto> GetTokenByLogin(LoginUserDto loginUserDto);
        Task<List<UserInClientDto>> GetUserList();
        Task<UserDto> GetUserInfoFromServer(Guid userGuid);
        Task<string> GetUserPasswordRecoveryCode(Guid userGuid);
        Task<string> GetRegistrationCode();
        Task<List<string>> GetUserRoles(Guid userGuid);
        List<string> GetUserRoleFromToken(string token);
        Task<MessageWithCode> SetUserRoles(Guid userId, List<string> roles);
        Task<MessageWithCode> SetUserPasswordChange(UserPasswordChangeDto userPasswordChangeDto);
        Task<MessageWithCode> SetUserPasswordByRecoveryCode(UserPasswordRecoveryDto userPasswordResetDto);
        Task<MessageWithCode> SetUserPasswordToRandom(Guid userGuid);
        Task<MessageWithCode> SetUserInfoChange(UserInfoChangeDto userInfoChangeDto);
        Task<MessageWithCode> SetUserInfo(SetUserInfoDto setUserInfoDto);
        void SaveToken(TokenDto token);

    }
}
