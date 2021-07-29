using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Helpers
{
    public class CustomRefreshTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public CustomRefreshTokenProvider(IDataProtectionProvider dataProtectionProvider,
        IOptions<RefreshTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger) 
            : base(dataProtectionProvider, options, logger)
        {

        }
    }

    public class RefreshTokenProviderOptions: DataProtectionTokenProviderOptions
    {
        public RefreshTokenProviderOptions()
        {
            Name = "RefreshTokenProvider";
            TokenLifespan = TimeSpan.FromDays(30);
        }
    }
}
