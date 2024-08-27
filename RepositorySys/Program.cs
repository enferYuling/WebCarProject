using System;
using Autofac.Extensions.DependencyInjection;
 
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Xml;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;
 
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
 
using Microsoft.AspNetCore.Http;
 
using System.Configuration;
using SqlSugar;


namespace WebCarProject
{
    public class Program
    {
        
        public static void Main(string[] args)
        {

          
            // 注入 单例 ISqlSugarClient
            Host.CreateDefaultBuilder(args)
             .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .ConfigureAppConfiguration(builder =>
              {
                  builder.AddJsonFile("appsettings.json", optional: true);
              })
             .ConfigureWebHostDefaults(webBuilder =>
             {
                 webBuilder
                 .UseStartup<Startup>()
                 // .UseUrls("http://*:6060")
                 .ConfigureLogging((hostingContext, builder) =>
                 {
                     //过滤掉系统默认的一些日志
                     builder.AddFilter("System", LogLevel.Error);
                     builder.AddFilter("Microsoft", LogLevel.Error);
                     

                     //可配置文件
                     //var path = Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config");
                     //builder.AddLog4Net(path);

                 });
             })

             // 生成承载 web 应用程序的 Microsoft.AspNetCore.Hosting.IWebHost。Build是WebHostBuilder最终的目的，将返回一个构造的WebHost，最终生成宿主。
             .Build()
             // 运行 web 应用程序并阻止调用线程, 直到主机关闭。
             // ※※※※ 有异常，查看 Log 文件夹下的异常日志 ※※※※  
             .Run();
        }

    }
}
