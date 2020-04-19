using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;



namespace UEditor.Server.Core.Handlers
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public abstract class Handler
    {
        protected readonly UEditorService Option;
        public Handler(HttpContext context, UEditorService option)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            this.Context = context;
            this.Option = option;
        }

        public abstract void Process();

        protected void WriteJson(object response)
        {
            string jsonpCallback = Request.Query["callback"];
            string json = JsonConvert.SerializeObject(response);
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                Response.Headers.Add("Content-Type", "text/plain");
                Response.WriteAsync(json);
            }
            else
            {
                Response.Headers.Add("Content-Type", "application/javascript");
                Response.WriteAsync(String.Format("{0}({1});", jsonpCallback, json));
            }
        }

        public HttpRequest Request { get; private set; }
        public HttpResponse Response { get; private set; }
        public HttpContext Context { get; private set; }
    }
}