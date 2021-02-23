using ESO_LangEditor.Core.Models;
using ESO_LangEditor.GUI.NetClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace ESO_LangEditorGUI.Services.AccessServer
{
    public class AccountService
    {

        public AccountService()
        {

        }

        public async void Login(LoginUserDto loginUserDto)
        {
            ApiAccess apiclient = new ApiAccess();
            var tokenDto = await apiclient.GetLoginToken(loginUserDto);

            if (tokenDto != null)
                SaveToken(tokenDto);
        }

        public async void LoginByToken()
        {
            ApiAccess apiclient = new ApiAccess();
            var tokenDto = await apiclient.GetRefreshedTokenByCurrentToken(new TokenDto 
            { 
                 AuthToken = App.LangConfig.UserAuthToken,
                 RefreshToken = App.LangConfig.UserRefreshToken,
            });

            if (tokenDto != null)
                SaveToken(tokenDto);
        }

        private void SaveToken(TokenDto tokenDto)
        {
            var username = DecodeTokenToGetUserName(tokenDto.AuthToken);
            var config = App.LangConfig;

            config.UserAuthToken = tokenDto.AuthToken;
            config.UserRefreshToken = tokenDto.RefreshToken;
            config.UserName = username;

            AppConfigClient.Save(config);
        }

        private string DecodeTokenToGetUserName(string token)
        {
            string userNameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

            var jsontoken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var jti = jsontoken.Claims.First(claim => claim.Type == userNameClaim).Value;
            
            return jti;
        }


    }
}
