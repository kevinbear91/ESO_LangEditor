using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditor.Core.Entities
{
    public class User : IdentityUser<Guid>
    {
        [StringLength(20)]
        public string UserNickName { get; set; }
        public string UserAvatarPath { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}
