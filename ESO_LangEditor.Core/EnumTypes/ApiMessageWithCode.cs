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
            switch (messageWithCode)
            {
                case ApiMessageWithCode.Success:
                    return "成功";
                case ApiMessageWithCode.Created:
                    return "已有存在项";
                case ApiMessageWithCode.NoContent:
                    return "没有获取到内容";
                case ApiMessageWithCode.TokenInvalid:
                    return "Token无效，请尝试重新登录";
                case ApiMessageWithCode.TokenExpired:
                    return "Token已过期，请尝试重新登录";
                case ApiMessageWithCode.TokenRequired:
                    return "Token缺失，请尝试重新登录";
                case ApiMessageWithCode.PasswordNotMatch:
                    return "密码不匹配";
                case ApiMessageWithCode.PasswordTooWeak:
                    return "弱密码";
                case ApiMessageWithCode.PasswordNeedToChange:
                    return "密码必须修改";
                case ApiMessageWithCode.PasswordRequired:
                    return "密码缺失";
                case ApiMessageWithCode.PasswordTooLong:
                    return "密码太长";
                case ApiMessageWithCode.PasswordChangeFailed:
                    return "密码修改失败";
                case ApiMessageWithCode.GenerateRegistrationCodeFailed:
                    return "生成注册码失败";
                case ApiMessageWithCode.RegistrationCodeNotFound:
                    return "注册码不存在";
                case ApiMessageWithCode.RegistrationCodeInvalid:
                    return "注册码无效";
                case ApiMessageWithCode.NoPermission:
                    return "没有权限执行当前操作";
                case ApiMessageWithCode.AuthenticationFailed:
                    return "验证失败";
                case ApiMessageWithCode.UserNameExisted:
                    return "用户名已存在";
                case ApiMessageWithCode.UserNickNameExisted:
                    return "用户昵称已存在";
                case ApiMessageWithCode.UserLocked:
                    return "当前用户已被锁定，请联系管理员";
                case ApiMessageWithCode.UserRoleSetFailed:
                    return "当前用户角色调整失败";
                case ApiMessageWithCode.UserInitFailed:
                    return "当前用户初始化失败";
                case ApiMessageWithCode.UserUpdateFailed:
                    return "当前用户更新失败";
                case ApiMessageWithCode.LangtextInReview:
                    return "当前条目已待审核，你没有权限更改";
                case ApiMessageWithCode.LangtextUpdateFailed:
                    return "当前条目更新失败";
                case ApiMessageWithCode.LangtextDeletedFailed:
                    return "当前条目删除失败";
                case ApiMessageWithCode.LangtextAddedFailed:
                    return "当前条目新增失败";
                case ApiMessageWithCode.LangtextNotOnServer:
                    return "当前条目不存在于服务器上";
                case ApiMessageWithCode.LangtextRevNumberUpdateFailed:
                    return "文本步进号更新失败";
                case ApiMessageWithCode.LangtextRevNumberNotExisted:
                    return "当前文本步进号不存在";
                default:
                    return "未知错误";
            }
        }
    }


}
