using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
  public  class VideoHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected", "成功连接到服务器。");
        }
        public async Task StartVideoConversion(string rtspUrl)
        {
            try
            {
                // 视频转码逻辑，与之前类似
                var ffmpegProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "ffmpeg.exe",
                        Arguments = $"-i {rtspUrl} -c:v libvpx-vp9 -c:a opus -f webm -g 5 -content_type video/webm pipe:1",
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    }
                };

                ffmpegProcess.Start();

                while (!ffmpegProcess.StandardOutput.EndOfStream)
                {
                    var data = new byte[1024];
                    var bytesRead = ffmpegProcess.StandardOutput.BaseStream.Read(data, 0, data.Length);
                    if (bytesRead > 0)
                    {
                        await Clients.All.SendAsync("VideoData", data.Take(bytesRead).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }
    }
}
