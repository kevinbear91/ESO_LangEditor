using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserProfileModifyBySelfDto
    {
        public Guid userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
