using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebCarProject.Filters;
using WebCarProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject.Controllers
{
    //[CustomActionFilter]
    public class HomeController : Controller
    {
        public string Name { get; set; }


        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult Index()
        {
            ////读取session用户名
            //string userName = HttpContext.Session.GetString("UserName");
            //if (string.IsNullOrEmpty(userName))
            //{
            //    //重定向到登录页
            //    //return new RedirectResult("/Account/LoginView");
            //    return  Redirect("/Account/LoginView");
            //}
            return View();
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult TestCookieAndSession()
        {
            HttpContext.Session.SetString("UserName", "zs");
            return Json("ok");
        }

    }
}
