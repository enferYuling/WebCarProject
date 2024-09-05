
using Entity;
using IBll;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using WebCarProject.Models;

namespace Bll
{
    
    public class APIBll:IAPIBll
    {
        public ISqlSugarClient db;
        public APIBll(ISqlSugarClient datadb)
        {
            this.db = datadb;
        }
    }
}
