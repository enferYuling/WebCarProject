
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
 
        public APIBll(ISqlSugarClient datadb)
        {
            this.db = datadb;
            this._ffmpegPath = @"D:\BaiduNetdiskDownload\ffmpeg-7.0.2-essentials_buildffmpeg-7.0.2-essentials_build\bin\ffmpeg.exe";
             
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
        /// <summary>
        /// 转码
        /// </summary>
        /// <param name="inputRstpUrl"></param>
        /// <param name="outputWebmPath"></param>
        /// <returns></returns>
        public async Task ConvertRstpToWebmAsync(string inputRstpUrl, string outputWebmPath)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i {inputRstpUrl} -c:v libvpx -c:a libopus -f webm {outputWebmPath}",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            };

            try
            {
                process.Start();
                process.BeginErrorReadLine();

                await Task.Run(() => process.WaitForExit());

                Console.WriteLine($"Conversion completed. Output file: {outputWebmPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
