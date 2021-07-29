using ESO_LangEditor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Services
{
    //From https://code-maze.com/using-refresh-tokens-in-asp-net-core-authentication/
    
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user);
        Task<string> GenerateRefreshToken(User user);
        Task<bool> VerifyRefreshToken(User user, string token);
        string GenerateRegistrationCode();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
