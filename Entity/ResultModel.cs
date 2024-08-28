using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCarProject.Models
{
    /// <summary>
    /// 结果对象类
    /// </summary>
    public class ResultModel
    {
        public int Code { get; set; } = 500;

        public string Msg { get; set; } = "失败";

        public object Data { get; set; }
    }
}
