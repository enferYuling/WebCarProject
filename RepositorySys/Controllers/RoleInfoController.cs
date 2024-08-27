using Bll;
using Entity; 
using IBll;
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
    public class RoleInfoController : Controller
    {
        //业务逻辑层
      
        IUserInfoBll _userInfoBll;
        public RoleInfoController(  IUserInfoBll userInfoBll)
        { 
            _userInfoBll = userInfoBll;
        }


        /// <summary>
        /// 角色列表视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult ListView()
        {
            return View();
        }

        /// <summary>
        /// 添加角色视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult CreateRoleInfoView()
        {
            return View();
        }

        /// <summary>
        /// 修改角色视图
        /// </summary>
        /// <returns></returns>
        public IActionResult UpdateRoleInfoView()
        {
            return View();
        }

        /// <summary>
        /// 绑定用户视图
        /// </summary>
        /// <returns></returns>
        public IActionResult BindUserInfoView()
        {
            return View();
        }

      
         
    }
}
