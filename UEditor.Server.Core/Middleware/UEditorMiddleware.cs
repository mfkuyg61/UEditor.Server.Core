using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UEditor.Server.Core.Handlers;

namespace UEditor.Server.Core
{
    /// <summary>
    /// 百度编辑器中间件
    /// </summary>
    public class UEditorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UEditorService Server;

        public UEditorMiddleware(RequestDelegate next, UEditorService server)
        {
            _next = next;
            this.Server = server;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.HasValue == true && context.Request.Path.Value == Server.Config.ControllerName)
            {
                #region 限制域名
                string url = context.Request.Headers["Referer"].ToString();
                bool isRefuse = true;
                foreach (var v in Server.Config.WithOrigins)
                {
                    Regex regex = new Regex(v);
                    if (regex.IsMatch(url))
                    {
                        isRefuse = false;
                        break;
                    }
                }
                if (isRefuse == true)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"UEditor,The request failed. Access is disabled across domains");
                    return;
                }
                #endregion

                try
                {
                    if (context.Request.Headers.ContainsKey("Origin"))
                    {//跨域限制
                        context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                        //context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                        context.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS,HEAD,PATCH");
                        context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"]);
                        context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                        if (context.Request.Method.Equals("OPTIONS"))
                        {
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            return;
                        }
                    }

                    Handler action = Server.GetHandler(context.Request.Query["action"], context);
                    action.Process();
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsync($"UEditor,The request failed,{ex}");
                }
            }
            else
            {
                await _next(context);
            }
        }

    }
}
