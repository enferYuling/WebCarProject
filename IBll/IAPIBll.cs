using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using FFmpeg.AutoGen;
using Microsoft.AspNetCore.Mvc;

namespace IBll
{
    public interface IAPIBll
    {
        public  Task<FileStreamResult> StreamRtspToHls(string rtspUrl, string outputPath);
    }
}
