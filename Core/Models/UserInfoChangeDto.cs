using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserInfoChangeDto
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserNickName { get; set; }
    }
}
