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
using System.IO;
using Newtonsoft.Json;


namespace WebCarProject.Controllers
{
    public class MapInfoController : Controller
    {
        IMapBll _mapBll;
        public MapInfoController(IMapBll mapBll)
        {
             _mapBll = mapBll;
        }

        public IActionResult ListView()
        {
           
            return View();
        }

        public IActionResult CreateView()
        {
           
            return View();
        }
        public IActionResult GetPageList(string filedate1,string filedate2,string filename,int page, int limit)
        {
            int count = 0;
            List<Pro_Map> list = _mapBll.GetPageList(filedate1,filedate2,filename,page,limit,ref count);
            return Json(new
            {
                code = 0,
                msg ="",
                count = count,
                data=list
            });
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadFile(IFormFile file)
        {
            int count = 0;
            ResultModel result = new ResultModel();
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
           
            path += file.FileName;
            string directory = Path.GetDirectoryName(path);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string newExtension = ".txt";
            string newFilePath = Path.Combine(directory, fileNameWithoutExtension + newExtension);
            var stream = new FileStream(newFilePath, FileMode.Create); 
           file.CopyTo(stream);
          
            //System.IO.File.Copy(path, newFilePath);
          //  System.IO.File.Delete(path);
            result.Code = 200;
            result.Msg = "上传成功";
            result.Data = newFilePath;

            return Json(result);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entityjson"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateInfo(string keyValue,string entityjson,string createuseraccount)
        
        {
            Pro_Map entity = JsonConvert.DeserializeObject<Pro_Map>(entityjson);
            ResultModel result = _mapBll.CreateInfo(keyValue,entity, createuseraccount);
            return Json(result);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DownloadFile()
        {
            string filePath = @"C:\web\uploads\GlobalMap.pcd";
            string fileName = Path.GetFileName(filePath);
            return File(filePath, "application/octet-stream", fileName);
        }
    }
}
