using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using FFmpeg.AutoGen;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebCarProject.Models;

namespace IBll
{
    public interface IAPIBll
    {
        public  Task  StartConversion(string rtspUrl);
        public ResultModel NetCoreVlc(string rtspUrl);
        public  Task ConvertRstpToWebmAsync(string inputRstpUrl, string outputWebmPath);
    }
}
