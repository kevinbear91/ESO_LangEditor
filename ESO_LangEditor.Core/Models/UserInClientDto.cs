using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public class UserInClientDto
    {
        public Guid Id { get; set; }
        public string UserNickName { get; set; }
        public string UserAvatarPath { get; set; }
        public int TranslatedCount { get; set; }
        public int InReviewCount { get; set; }
        public int RemovedCount { get; set; }
    }
}
