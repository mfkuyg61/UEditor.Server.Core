# UEditor.Server.Core
百度UEditor(图片、文件等)上传所依赖的后端服务，支持.Net Core 2.0+，简单易用，

#支持前后端分离

# 使用教程
当前后台服务接口地址为http://localhost:58898时

# 1.建议使用NuGet安装：Install-Package UEditor.Server.Core -Version 1.0.2

# 2.在ConfigureServices中添加
services.AddUEditor(a =>  
{  
    a.WebRootPath = _hostingEnvironment.WebRootPath;//为_hostingEnvironment为IHostingEnvironment  
    a.ConfigFile = "ueditor\\ueditor.json";//配置文件相对路径  
    a.IsCache = false;//不使用缓存  
    a.ControllerName = "/controller";//后台接口  
    a.WithOrigins = new string[] { "http://localhost:58898" };//访问白名单  
});  

# 3.在Configure中添加  
app.UseUEditor();//百度编辑器,请放在app.UseCors()前面  
 
 
# 4.UEditor配置,以UEditor.Server.Core.Demo为例  
ueditor.config.js中配置  
window.UEDITOR_HOME_URL = '/ueditor/'  
window.UEDITOR_CONFIG={  
    serverUrl:'http://localhost:58898/controller'  
}  

# 5.注意:如果是前后端分离配置文件需要修改,否则空字符即可,demo改名为ueditor.json  

{  
    "imageUrlPrefix":"http://localhost:58898/",  
    "scrawlUrlPrefix": "http://localhost:58898/",   
    "snapscreenUrlPrefix": "http://localhost:58898/",  
    "catcherUrlPrefix": "http://localhost:58898/",  
    "videoUrlPrefix": "http://localhost:58898/",  
    "fileUrlPrefix": "http://localhost:58898/",  
    "imageManagerUrlPrefix": "http://localhost:58898/",  
    "fileManagerUrlPrefix": "http://localhost:58898/",  
}  

# 6.请使用Demo中的ueditor.all.js,官方的有iframe跨域问题
开始修改行号:24478-24579
修改前源代码:24580-24645

