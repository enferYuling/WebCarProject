
using Entity;
using IBll;
using Microsoft.AspNetCore.Mvc.Formatters;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Text;
using WebCarProject.Models;
using Common;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using FFMpegCore;
using Microsoft.AspNetCore.Mvc;

namespace Bll
{
    
    public class APIBll:IAPIBll
    {

        public ISqlSugarClient db;
        private readonly string _ffmpegPath;
 
        public APIBll(ISqlSugarClient datadb)
        {
            this.db = datadb;
            this._ffmpegPath = @"D:\BaiduNetdiskDownload\ffmpeg-7.0.2-essentials_buildffmpeg-7.0.2-essentials_build\bin\ffmpeg.exe";
        }
        /// <summary>
        /// 转码
        /// </summary>
        /// <param name="rtspUrl"></param>
        /// <param name="outputPath">输出地址</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

       public async Task<FileStreamResult>  StreamRtspToHls(string rtspUrl, string outputPath)
        {
            try
            {
                 rtspUrl = "your_rtsp_url";
                //string tempOutputFilePath = Path.GetTempFileName() + ".webm";

                await FFMpegArguments
                   .FromFileInput(rtspUrl)
                   .OutputToFile(outputPath, true, options => options
                       .WithVideoCodec("libvpx-vp9")
                       .WithAudioCodec("opus")
                       .ForceFormat("webm"))
                   .ProcessAsynchronously();

                var memoryStream = new MemoryStream();
                using (var fileStream = new FileStream(outputPath, FileMode.Open))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }

                System.IO.File.Delete(outputPath);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return new FileStreamResult(memoryStream, "video/webm");
            }
            catch (Exception ex)
            {
               // throw new Exception(ex.Message);
                return null;
            }
            
        }
    }
}
