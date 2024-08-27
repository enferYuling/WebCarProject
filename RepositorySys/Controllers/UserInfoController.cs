using Bll;
using Entity; 
using IBll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCarProject.Filters;
using WebCarProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject.Controllers
{
    //[CustomActionFilter]
    public class UserInfoController : Controller
    {
        //创建业务逻辑层
        IUserInfoBll _userInfoBll;
      

        public UserInfoController(IUserInfoBll userInfoBll )
        {
            _userInfoBll = userInfoBll; 
        }

        /// <summary>
        /// 数据列表视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult ListView()
        {

            return View();
        }

        /// <summary>
        /// 创建用户视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult CreateUserInfoView()
        {
            return View();
        }

        /// <summary>
        /// 修改用户视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult UpdateUserInfoView()
        {
            return View();
        } 

    }
}
