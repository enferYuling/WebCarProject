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
    public class DepartmentInfoController : Controller
    {
        //创建业务逻辑层
        IUserInfoBll _userInfoBll;
        

        public DepartmentInfoController(IUserInfoBll userInfoBll )
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
        /// 创建部门视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult CreateDepartmentInfoView()
        {
            return View();
        }

        /// <summary>
        /// 修改部门视图
        /// </summary>
        /// <returns></returns>
        //[CustomActionFilter]
        public IActionResult UpdateDepartmentInfoView()
        {
            return View();
        }
         
    }
}
