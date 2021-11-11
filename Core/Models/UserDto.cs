using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserDto
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string UserNickName { get; set; }
        public int TranslatedCount { get; set; }
        public int InReviewCount { get; set; }
        public int RemovedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
