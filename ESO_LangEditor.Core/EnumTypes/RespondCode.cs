
namespace ESO_LangEditor.Core.EnumTypes
{
    public enum RespondCode : int
    {
        Success = 200,
        Created = 201,
        NoContent = 202,
        TokenInvalid = 301,
        TokenExpired = 302,
        TokenRequired = 303,
        PasswordNotMatch = 350,
        PasswordTooWeak = 351,
        PasswordNeedToChange = 352,
        PasswordRequired = 353,
        PasswordTooLong = 354,
        PasswordChangeFailed = 355,
        GenerateRegistrationCodeFailed = 370,
        RegistrationCodeNotFound = 371,
        RegistrationCodeInvalid = 372,
        NoPermission = 400,
        AuthenticationFailed = 401,
        UserNameExisted = 410,
        UserNickNameExisted = 411,
        UserLocked = 412,
        UserRoleSetFailed = 413,
        UserInitFailed = 414,
        UserUpdateFailed = 415,
        UserNotFound = 416,
        UserRegistrationFailed = 419,
        RoleAddFailed = 420,
        RoleExisted = 421,
        LangtextInReview = 450,
        LangtextUpdateFailed = 451,
        LangtextDeletedFailed = 452,
        LangtextAddedFailed = 453,
        LangtextNotOnServer = 454,
        LangtextRevNumberUpdateFailed = 455,
        LangtextRevNumberNotExisted = 456,
        LangtextReviewFailed = 457,

    }
    public static class ApiRespondCodeExtensions
    {
        public static string ApiRespondCodeString(this RespondCode messageWithCode)
        {
            return messageWithCode switch
            {
                RespondCode.Success => "成功",
                RespondCode.Created => "已有存在项",
                RespondCode.NoContent => "没有获取到内容",
                RespondCode.TokenInvalid => "Token无效，请尝试重新登录",
                RespondCode.TokenExpired => "Token已过期，请尝试重新登录",
                RespondCode.TokenRequired => "Token缺失，请尝试重新登录",
                RespondCode.PasswordNotMatch => "密码不匹配",
                RespondCode.PasswordTooWeak => "弱密码",
                RespondCode.PasswordNeedToChange => "密码必须修改",
                RespondCode.PasswordRequired => "密码缺失",
                RespondCode.PasswordTooLong => "密码太长",
                RespondCode.PasswordChangeFailed => "密码修改失败",
                RespondCode.GenerateRegistrationCodeFailed => "生成注册码失败",
                RespondCode.RegistrationCodeNotFound => "注册码不存在",
                RespondCode.RegistrationCodeInvalid => "注册码无效",
                RespondCode.NoPermission => "没有权限执行当前操作",
                RespondCode.AuthenticationFailed => "验证失败",
                RespondCode.UserNameExisted => "用户名已存在",
                RespondCode.UserNickNameExisted => "用户昵称已存在",
                RespondCode.UserLocked => "当前用户已被禁止登录，请联系管理员",
                RespondCode.UserRoleSetFailed => "当前用户角色调整失败",
                RespondCode.UserInitFailed => "当前用户初始化失败",
                RespondCode.UserUpdateFailed => "当前用户更新失败",
                RespondCode.LangtextInReview => "当前条目已待审核，你没有权限更改",
                RespondCode.LangtextUpdateFailed => "当前条目更新失败",
                RespondCode.LangtextDeletedFailed => "当前条目删除失败",
                RespondCode.LangtextAddedFailed => "当前条目新增失败",
                RespondCode.LangtextNotOnServer => "当前条目不存在于服务器上",
                RespondCode.LangtextRevNumberUpdateFailed => "文本步进号更新失败",
                RespondCode.LangtextRevNumberNotExisted => "当前文本步进号不存在",
                RespondCode.UserNotFound => "用户不存在",
                RespondCode.UserRegistrationFailed => "账号注册失败，请检查是否符合要求",
                RespondCode.RoleAddFailed => "角色添加失败",
                RespondCode.RoleExisted => "角色已存在",
                RespondCode.LangtextReviewFailed => "文本审核失败",
                _ => "未知错误",
            };
        }
    }


}
