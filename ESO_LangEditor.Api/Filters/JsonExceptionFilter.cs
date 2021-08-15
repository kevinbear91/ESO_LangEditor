using ESO_LangEditor.API.Helpers;
using ESO_LangEditor.Core.Models;
using ESO_LangEditorApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.API.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        public IWebHostEnvironment Environment { get; }
        public ILogger Logger { get; }

        public JsonExceptionFilter(IWebHostEnvironment env, ILogger<Program> logger)
        {
            Environment = env;
            Logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            var error = new MessageWithCode();
            if (Environment.IsDevelopment())
            {
                error.Message = context.Exception.Message + "\n" + context.Exception.ToString();
                //error.Detail = context.Exception.ToString();
            }
            else
            {
                error.Message = "服务器出错" + "\n" + context.Exception.Message;
                //error.Detail = context.Exception.Message;
            }

            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"服务发生异常: {context.Exception.Message}");
            sb.AppendLine(context.Exception.ToString());
            Logger.LogCritical(sb.ToString());
        }
    }
}
