﻿ 
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
using System.IO;
using FFMpegCore;
using Common;
using Microsoft.AspNetCore.SignalR;

namespace WebCarProject.Controllers
{

    public class APIController : Controller
    {
        //业务访问层对象
        IAPIBll  _APIBll;
        private readonly IHubContext<VideoHub> _hubContext;

        public APIController(IAPIBll APIBll, IHubContext<VideoHub> hubContext)
        {
            _APIBll = APIBll;
            _hubContext = hubContext;
        }
      
        /// <summary>
        /// 地图标注
        /// </summary>
        /// <returns></returns>
        public IActionResult loadjgdt()
        {
            return View();
        }
        /// <summary>
        /// 地图查看
        /// </summary>
        /// <returns></returns>
        public IActionResult jglddt()
        {
            return View();
        }
        /// <summary>
        /// 航拍地图
        /// </summary>
        /// <returns></returns>
        public IActionResult HPDT()
        {
            return View();
        }
        /// <summary>
        /// 视频推流
        /// </summary>
        /// <returns></returns>
        public IActionResult Testsp()
        {
            return View();
        }
        /// <summary>
        /// 保存标注文件到服务器
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveToServer()
        {
            try
            {
                var httpRequest = Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var postedFile = httpRequest.Form.Files[0];
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    //  var filePath = Path.Combine("your_server_path", postedFile.FileName);
                    path += postedFile.FileName;
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }
                    return Ok("Canvas saved successfully.");
                }
                else
                {
                    return BadRequest("No file uploaded.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="rtspUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> PlayVideo([FromBody] string rtspUrl)
        {
            await _hubContext.Clients.All.SendAsync("StartVideoConversion", rtspUrl);
            return Ok();

        }
        [HttpPost]
        public async Task<IActionResult> PlayVideo2([FromBody] string rtspUrl)
        {
            await _APIBll.StartConversion(rtspUrl);
            return Ok();

        }
    }
}
