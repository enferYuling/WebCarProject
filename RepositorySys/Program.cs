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

          
            // ע�� ���� ISqlSugarClient
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
                     //���˵�ϵͳĬ�ϵ�һЩ��־
                     builder.AddFilter("System", LogLevel.Error);
                     builder.AddFilter("Microsoft", LogLevel.Error);
                     

                     //�������ļ�
                     //var path = Path.Combine(Directory.GetCurrentDirectory(), "Log4net.config");
                     //builder.AddLog4Net(path);

                 });
             })

             // ���ɳ��� web Ӧ�ó���� Microsoft.AspNetCore.Hosting.IWebHost��Build��WebHostBuilder���յ�Ŀ�ģ�������һ�������WebHost����������������
             .Build()
             // ���� web Ӧ�ó�����ֹ�����߳�, ֱ�������رա�
             // �������� ���쳣���鿴 Log �ļ����µ��쳣��־ ��������  
             .Run();
        }

    }
}
