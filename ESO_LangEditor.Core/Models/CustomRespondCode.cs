using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.Models
{
    public enum CustomRespondCode : int
    {
        InitAccountRequired = 401,
        NickNameCanNotBeNull = 402,
        UserNameCanNotBeNull = 403,
        PasswordNeedToReset = 404,
        TokenInvaild = 405,

    }
}
