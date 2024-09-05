using Bll;
using Common;
 
using Entity;
using IBll;
 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebCarProject.Filters;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // private static readonly string connectionString = "Data Source=.;Database=CarProject;User Id=sa;Password=123456;";
        private static readonly string connectionString = "Data Source=8.137.119.17;Database=CarProject;User Id=sa;Password=Hbjkj@#123;";
        /// <summary>
        /// ����ע��
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //ע�������ģ�AOP������Ի�ȡIOC����������ֳɿ�ܱ���Furion���Բ�д��һ��
            services.AddHttpContextAccessor();


            //ȫ�����ù�����
            services.AddMvcCore(options =>
            {
                options.Filters.Add<CustomActionFilterAttribute>();
            });

            //ע��session
            services.AddSession();
            //ע��SqlSugar
            services.AddSingleton<ISqlSugarClient>(s =>
            {
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugar.DbType.SqlServer,
                    ConnectionString = connectionString,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute
                },
               db =>
               {
                   //�����������ã�������������Ч
                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {
                       //��ȡ��IOC���������
                       var appServive = s.GetService<IHttpContextAccessor>();
                      // var obj = appServive?.HttpContext?.RequestServices.GetService<SqlSugarConfig>();
                      // Console.WriteLine("AOP" + obj.GetHashCode());
                   };
               });
                return sqlSugar;
            });


            //ע�������Ķ���
            //��1�����Ͳ�����������ʲô���ͣ����캯�����������ͣ�
            //��2�����Ͳ������������������

            //IOC ��������
            //Scoped����һ���������ǵ�����һ�������оʹ���һ���µĶ��������ͬһ�������������õ�����ͬһ�����󡣵ڶ����ֻᴴ��һ���µĶ���
            //Transient:ÿ��������Ҫ���󣬶��ᴴ��һ��ȫ�¶���ʵ����
            //Singleton:һ��������Ҫ����ͻᴴ��һ�����󣬺���������Ҫ���󶼻�ʹ�õ�һ�δ������Ǹ�����


            services.AddScoped<IUserInfoBll, UserInfoBll>();
            services.AddScoped<IMapBll, MapBll>();
            services.AddScoped<IAPIBll, APIBll>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("https://webapi.amap.com", "https://webst02.is.autonavi.com", "https://webst01.is.autonavi.com", "https://webst03.is.autonavi.com") // �����Դ����
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                 .WithExposedHeaders("Content-Disposition")
                 .SetIsOriginAllowedToAllowWildcardSubdomains()
                 .SetIsOriginAllowed(isOriginAllowed =>
                 {
                     // ��������Ϊ��
                     return isOriginAllowed == null || isOriginAllowed == "https://webapi.amap.com"|| isOriginAllowed== "https://webst02.is.autonavi.com"|| isOriginAllowed== "https://webst01.is.autonavi.com"|| isOriginAllowed== "https://webst03.is.autonavi.com"|| isOriginAllowed=="null";
                 });
                });
            });
        }

        /// <summary>
        /// ����ʹ�ã�core�м����
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();  //ʹ�þ�̬�ļ�

            app.UseRouting();//ʹ��·��
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthorization();//��Ȩ

            //ʹ��session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=LoginView}/{id?}");
                   // pattern: "{controller=Account}/{action=LoginView}/{id?}");
            });
        
           
        }
    }
}
