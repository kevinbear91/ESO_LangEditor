using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class UserProfileModifyBySelfDto
    {
        public Guid userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
