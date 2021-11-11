using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserPasswordChangeDto
    {
        public Guid UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirm { get; set; }
    }
}
