using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace UEditor.Server.Core.Handlers
{
    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : Handler
    {
        public ConfigHandler(HttpContext context, UEditorService option) : base(context,option) { }

        public override void Process()
        {
            WriteJson(Option.Items);
        }
    }
}