using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UEditor.Server.Core.Handlers;

namespace UEditor.Server.Core
{
    /// <summary>
    /// 百度编辑器服务
    /// </summary>
    public class UEditorService
    {
        public UEditorOptions Config;
        public UEditorService(IOptions<UEditorOptions> option)
        {
            this.Config = option.Value;
        }

        /// <summary>
        /// 构造配置
        /// </summary>
        /// <returns></returns>
        private JObject BuildItems()
        {
            var json = File.ReadAllText(Path.Combine(Config.WebRootPath, Config.ConfigFile));
            return JObject.Parse(json);
        }
        /// <summary>
        /// 配置集合
        /// </summary>
        public JObject Items
        {
            get
            {
                if (Config.IsCache || _Items == null)
                {
                    _Items = BuildItems();
                }
                return _Items;
            }
        }
        private JObject _Items;

        public T GetValue<T>(string key)
        {
            return Items[key].Value<T>();
        }

        public String[] GetStringList(string key)
        {
            return Items[key].Select(x => x.Value<String>()).ToArray();
        }

        public String GetString(string key)
        {
            return GetValue<String>(key);
        }

        public int GetInt(string key)
        {
            return GetValue<int>(key);
        }

        public Handler GetHandler(string actionName, HttpContext context)
        {
            Handler action;
            switch (actionName)
            {
                case "config":
                    action = new ConfigHandler(context, this);
                    break;
                case "uploadimage"://上传图片
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = this.GetStringList("imageAllowFiles"),
                        PathFormat = this.GetString("imagePathFormat"),
                        SizeLimit = this.GetInt("imageMaxSize"),
                        UploadFieldName = this.GetString("imageFieldName"),
                    }, this);
                    break;
                case "uploadscrawl"://涂鸦图片
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = new string[] { ".png" },
                        PathFormat = this.GetString("scrawlPathFormat"),
                        SizeLimit = this.GetInt("scrawlMaxSize"),
                        UploadFieldName = this.GetString("scrawlFieldName"),
                        Base64 = true,
                        Base64Filename = "scrawl.png"
                    }, this);
                    break;
                case "uploadvideo"://上传视频
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = this.GetStringList("videoAllowFiles"),
                        PathFormat = this.GetString("videoPathFormat"),
                        SizeLimit = this.GetInt("videoMaxSize"),
                        UploadFieldName = this.GetString("videoFieldName")
                    }, this);
                    break;
                case "uploadfile"://上传文件
                    action = new UploadHandler(context, new UploadConfig()
                    {
                        AllowExtensions = this.GetStringList("fileAllowFiles"),
                        PathFormat = this.GetString("filePathFormat"),
                        SizeLimit = this.GetInt("fileMaxSize"),
                        UploadFieldName = this.GetString("fileFieldName")
                    }, this);
                    break;
                case "listimage"://列出指定目录下的图片
                    action = new ListFileManager(context, this, this.GetString("imageManagerListPath"), this.GetStringList("imageManagerAllowFiles"));
                    break;
                case "listfile"://列出指定目录下的文件
                    action = new ListFileManager(context, this, this.GetString("fileManagerListPath"), this.GetStringList("fileManagerAllowFiles"));
                    break;
                case "catchimage"://截图工具
                    action = new CrawlerHandler(context, this);
                    break;
                default:
                    action = new NotSupportedHandler(context, this);
                    break;
            }
            return action;
        }
    }
}
