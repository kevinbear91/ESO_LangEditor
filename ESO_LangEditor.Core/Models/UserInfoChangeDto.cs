using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class UserInfoChangeDto
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserNickName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public bool IsChangedPassword { get; set; }
    }
}
