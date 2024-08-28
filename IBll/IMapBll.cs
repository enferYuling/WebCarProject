using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Text;

using Entity;
using WebCarProject.Models;

namespace IBll
{
    public interface IMapBll
    {
        public List<Pro_Map> GetPageList(string filedate1, string filedate2, string filename, int page, int limit,ref int count);
        public ResultModel CreateInfo(string keyValue, Pro_Map entity,string createuseraccount);
    }
}
