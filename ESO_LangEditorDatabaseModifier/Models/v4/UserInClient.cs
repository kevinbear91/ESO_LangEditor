using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditorDatabaseModifier.Model.v4
{
    public class UserInClient
    {
        public Guid Id { get; set; }
        public string UserNickName { get; set; }
        public string UserAvatarPath { get; set; }
        public int TranslatedCount { get; set; }
        public int InReviewCount { get; set; }
        public int RemovedCount { get; set; }
    }
}
