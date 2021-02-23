using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class LoginUserDto
    {
        //[Required]
        //[EmailAddress]
        //public string Email { get; set; }

        public Guid UserID { get; set; }

        public string UserName { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public bool LoginWithId { get; set; }

        public string RefreshToken { get; set; } 

        public DateTime RefreshTokenExpireTime { get; set; }

    }
}
