 
using Entity;
using IBll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebCarProject.Filters;
using WebCarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject.Controllers
{

    public class AccountController : Controller
    {
        //业务访问层对象
        IUserInfoBll _userInfoBll;

        public AccountController(IUserInfoBll userInfoBll)
        {
            _userInfoBll = userInfoBll;
        }



        /// <summary>
        /// 登录页
        /// </summary>
        /// <returns></returns>
        public IActionResult LoginView()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login(string account, string password)
        {
            ResultModel res = new ResultModel();

            //参数验证
            if (string.IsNullOrEmpty(account))
            {
                res.Code = 501;
                res.Msg = "账号不能为空";
                return Json(res);
            }
            if (string.IsNullOrEmpty(password))
            {
                res.Code = 501;
                res.Msg = "密码不能为空";
                return Json(res);
            }

            string msg;
            //验证账号密码
            Base_User userInfo = _userInfoBll.Login(account, password,out msg);
            if (userInfo != null)
            {
                res.Code = 200;

                //登录成功后保存用户名到session中
                HttpContext.Session.SetString("UserName", userInfo.realname);
                HttpContext.Session.SetString("account", userInfo.account);
                HttpContext.Session.SetString("userid", userInfo.userid);
                //返回用户名字
                res.Data = userInfo;
            }
            res.Msg = msg;

            return Json(res);
        }

    }
}
