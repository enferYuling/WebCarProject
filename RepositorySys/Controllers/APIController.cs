 
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

    public class APIController : Controller
    {
        //业务访问层对象
        IAPIBll  _APIBll;

        public APIController(IAPIBll APIBll)
        {
            _APIBll = APIBll;
        }
        /// <summary>
        /// 地图标注
        /// </summary>
        /// <returns></returns>
        public IActionResult loadjgdt()
        {
            return View();
        }

    }
}
