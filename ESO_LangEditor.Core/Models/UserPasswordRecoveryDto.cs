using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class UserPasswordRecoveryDto
    {
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
        public string RecoveryCode { get; set; }
    }
}
