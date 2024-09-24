using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebCarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject.Filters
{
    /// <summary>
    /// 自定义方法过滤器 (特性)
    /// </summary>
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        /// <summary>
        /// 在方法执行之前就会执行下面方法
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //拿到你方法头上有没有API特性，拿你就是接口（反射，特性）

            //读取session用户名
            string userName = context.HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(userName))
            {
                //拿到请求的地址
                string path = context.HttpContext.Request.Path.ToString().ToLower();

                //如果是登录页或者登录接口，都不用验证
                if (!path.Contains("login"))
                {
                    if (path.Contains("view") || path.Contains("index") || path == "/")   //1、一种页面过滤
                    {
                        //重定向到登录页
                        //return Redirect("/Account/LoginView");
                        //context.HttpContext.Response.Redirect("/Account/LoginView");
                        context.Result = new RedirectResult("/Account/LoginView");

                    } else if (path.Contains("api/")|| path.Contains("uploads"))
                    {
                          
                    }
                    else  //2、接口的过滤
                    {
                        //context.Result = new JsonResult(new ResultModel
                        //{
                        //    Code = 401,
                        //    Msg = "请重新登录"
                        //});
                    }
                }


            }

        }
    }
}
