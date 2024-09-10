
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
using Vlc.DotNet.Core;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Bll
{
    
    public class APIBll:IAPIBll
    {

        public ISqlSugarClient db;
        private readonly IHubContext<VideoHub> _hubContext;
        private readonly string _ffmpegPath;
 
        public APIBll(ISqlSugarClient datadb, IHubContext<VideoHub> hubContext)
        {
            this.db = datadb;
            this._ffmpegPath = @"D:\BaiduNetdiskDownload\ffmpeg-7.0.2-essentials_buildffmpeg-7.0.2-essentials_build\bin\ffmpeg.exe";
            _hubContext = hubContext;
        }
        /// <summary>
        /// 转码
        /// </summary>
        /// <param name="rtspUrl"></param>
        /// <param name="outputPath">输出地址</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task StartConversion(string rtspUrl)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("D:\\BaiduNetdiskDownload\\ffmpeg-7.0.2-full_build\\bin\\ffmpeg.exe")
                {
                    
                    Arguments = $"-i {rtspUrl} -c:v libvpx-vp9 -c:a opus -f webm -g 5 -content_type video/webm pipe:1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };
                var ffmpegProcess = new Process
                {
                    StartInfo = startInfo
                };

                ffmpegProcess.Start();

                while (!ffmpegProcess.StandardOutput.EndOfStream)
                {
                    var data = new byte[1024];
                    //var output = ffmpegProcess.StandardOutput.ReadLine();
                    //ffmpegProcess.Kill();
                    var bytesRead = ffmpegProcess.StandardOutput.BaseStream.Read(data, 0, data.Length);
                    if (bytesRead > 0)
                    {
                        await _hubContext.Clients.All.SendAsync("VideoData", data.Take(bytesRead).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                string msg=ex.Message;
            }
        }
        /// <summary>
        /// .net core播放视频
        /// </summary>
        /// <param name="rtspUrl"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ResultModel NetCoreVlc(string rtspUrl)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(rtspUrl);

                VlcMediaPlayer player = new VlcMediaPlayer(directoryInfo);
                player.Play();
                resultModel.Code = 200;
                return resultModel;
            }
            catch (Exception ex)
            {
                return resultModel;
            }
 
        }
    }
}
