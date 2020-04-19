using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace UEditor.Server.Core
{
    public static class UEditorExtensions
    {
        /// <summary>
        /// 使用百度编辑器中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseUEditor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UEditorMiddleware>();
        }

        /// <summary>
        /// 添加百度编辑器服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IServiceCollection AddUEditor(this IServiceCollection services, Action<UEditorOptions> options)
        {
            var option = new UEditorOptions();
            options(option);
            services.Configure(options);
            services.AddSingleton(typeof(UEditorService));
            return services;
        }
    }
}
