using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UEditor.Server.Core
{
    /// <summary>
    /// 百度编辑器选项
    /// </summary>
    public class UEditorOptions
    {
        /// <summary>
        /// 访问接口,以/为前缀，默认等于/controller
        /// </summary>
        public string ControllerName { get; set; } = "/controller";
        /// <summary>
        /// 网站静态资源目录(wwwroot)目录
        /// </summary>
        public string WebRootPath { get; set; }
        /// <summary>
        /// 配置文件相对路径
        /// </summary>
        public string ConfigFile { set; get; }
        /// <summary>
        /// 是否加缓存,为true时,配置文件会缓存在内存中,默认true
        /// </summary>
        public bool IsCache { set; get; } = true;
        /// <summary>
        /// 允许请求的域名
        /// </summary>
        public string[] WithOrigins { get; set; }
    }
}