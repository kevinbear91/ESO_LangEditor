using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string UserNickName { get; set; }
    }
}
