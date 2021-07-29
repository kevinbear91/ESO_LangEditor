using System;
using System.Collections.Generic;
using System.Text;

namespace ESO_LangEditor.Core.EnumTypes
{
    public enum ApiMessageWithCode : int
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
        LangtextInReview = 450,
        LangtextUpdateFailed = 451,
        LangtextDeletedFailed = 452,
        LangtextAddedFailed = 453,
        LangtextNotOnServer = 454,
        LangtextRevNumberUpdateFailed = 455,
        LangtextRevNumberNotExisted = 456,

    }
    public static class ApiMessageWithCodeExtensions
    {
        public static string ApiMessageCodeString(this ApiMessageWithCode messageWithCode)
        {
            return messageWithCode switch
            {
                ApiMessageWithCode.Success => "成功",
                ApiMessageWithCode.Created => "已有存在项",
                ApiMessageWithCode.NoContent => "没有获取到内容",
                ApiMessageWithCode.TokenInvalid => "Token无效，请尝试重新登录",
                ApiMessageWithCode.TokenExpired => "Token已过期，请尝试重新登录",
                ApiMessageWithCode.TokenRequired => "Token缺失，请尝试重新登录",
                ApiMessageWithCode.PasswordNotMatch => "密码不匹配",
                ApiMessageWithCode.PasswordTooWeak => "弱密码",
                ApiMessageWithCode.PasswordNeedToChange => "密码必须修改",
                ApiMessageWithCode.PasswordRequired => "密码缺失",
                ApiMessageWithCode.PasswordTooLong => "密码太长",
                ApiMessageWithCode.PasswordChangeFailed => "密码修改失败",
                ApiMessageWithCode.GenerateRegistrationCodeFailed => "生成注册码失败",
                ApiMessageWithCode.RegistrationCodeNotFound => "注册码不存在",
                ApiMessageWithCode.RegistrationCodeInvalid => "注册码无效",
                ApiMessageWithCode.NoPermission => "没有权限执行当前操作",
                ApiMessageWithCode.AuthenticationFailed => "验证失败",
                ApiMessageWithCode.UserNameExisted => "用户名已存在",
                ApiMessageWithCode.UserNickNameExisted => "用户昵称已存在",
                ApiMessageWithCode.UserLocked => "当前用户已被锁定，请联系管理员",
                ApiMessageWithCode.UserRoleSetFailed => "当前用户角色调整失败",
                ApiMessageWithCode.UserInitFailed => "当前用户初始化失败",
                ApiMessageWithCode.UserUpdateFailed => "当前用户更新失败",
                ApiMessageWithCode.LangtextInReview => "当前条目已待审核，你没有权限更改",
                ApiMessageWithCode.LangtextUpdateFailed => "当前条目更新失败",
                ApiMessageWithCode.LangtextDeletedFailed => "当前条目删除失败",
                ApiMessageWithCode.LangtextAddedFailed => "当前条目新增失败",
                ApiMessageWithCode.LangtextNotOnServer => "当前条目不存在于服务器上",
                ApiMessageWithCode.LangtextRevNumberUpdateFailed => "文本步进号更新失败",
                ApiMessageWithCode.LangtextRevNumberNotExisted => "当前文本步进号不存在",
                ApiMessageWithCode.UserNotFound => "用户不存在",
                _ => "未知错误",
            };
        }
    }


}
