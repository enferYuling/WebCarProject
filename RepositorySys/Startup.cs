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
        /// 配置注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            //注册上下文：AOP里面可以获取IOC对象，如果有现成框架比如Furion可以不写这一行
            services.AddHttpContextAccessor();


            //全局配置过滤器
            services.AddMvcCore(options =>
            {
                options.Filters.Add<CustomActionFilterAttribute>();
            });

            //注册session
            services.AddSession();
            //注册SqlSugar
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
                   //单例参数配置，所有上下文生效
                   db.Aop.OnLogExecuting = (sql, pars) =>
                   {
                       //获取作IOC作用域对象
                       var appServive = s.GetService<IHttpContextAccessor>();
                      // var obj = appServive?.HttpContext?.RequestServices.GetService<SqlSugarConfig>();
                      // Console.WriteLine("AOP" + obj.GetHashCode());
                   };
               });
                return sqlSugar;
            });


            //注册上下文对象
            //第1个泛型参数：遇到是什么类型（构造函数遇到的类型）
            //第2个泛型参数：创建对象的类型

            //IOC 生命周期
            //Scoped：在一个请求中是单例。一次请求中就创建一个新的对象，如果是同一个请求，则容器拿到的是同一个对象。第二次又会创建一个新的对象。
            //Transient:每次问容器要对象，都会创建一个全新对象（实例）
            //Singleton:一次问容器要对象就会创建一个对象，后面问容器要对象都会使用第一次创建的那个对象。


            services.AddScoped<IUserInfoBll, UserInfoBll>();
            services.AddScoped<IMapBll, MapBll>();

            //services.AddScoped<IDepartmentInfoBll, DepartmentInfoBll>();
            //services.AddScoped<IDepartmentInfoDal, DepartmentInfoDal>();

            //services.AddScoped<IRoleInfoBll, RoleInfoBll>();
            //services.AddScoped<IRoleInfoDal, RoleInfoDal>();

            //services.AddScoped<IR_UserInfo_RoleInfoDal, R_UserInfo_RoleInfoDal>();
        }

        /// <summary>
        /// 配置使用（core中间件）
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
            app.UseStaticFiles();  //使用静态文件

            app.UseRouting();//使用路由

            app.UseAuthorization();//授权

            //使用session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=LoginView}/{id?}");
            });
        }
    }
}
