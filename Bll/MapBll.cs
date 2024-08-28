
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
    public class MapBll : IMapBll
    {
        public ISqlSugarClient db;
        public MapBll(ISqlSugarClient datadb)
        {
            this.db = datadb;
        }
        public List<Pro_Map> GetPageList(string filedate1, string filedate2, string filename, int page, int limit, ref int count)
        {
            var exp = Expressionable.Create<Pro_Map>();
            exp.AndIF(!string.IsNullOrEmpty(filedate1), it => it.filetime >= filedate1.ObjToDate());//.AndIF 是条件成立才会拼接 
            exp.AndIF(!string.IsNullOrEmpty(filedate2), it => it.filetime <= filedate2.ObjToDate());//.AndIF 是条件成立才会拼接 
            exp.AndIF(!string.IsNullOrEmpty(filename), it => it.mapName.Contains(filename));//.AndIF 是条件成立才会拼接 
            List<Pro_Map> list = this.db.Queryable<Pro_Map>().Where(A => A.Enabled == 1).ToPageList(page, limit, ref count);
            return list;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
       public ResultModel  CreateInfo(string keyValue, Pro_Map entity, string createuseraccount)
        {
            ResultModel result = new ResultModel();
            if (string.IsNullOrEmpty(keyValue))//新建
            {
                entity.Create();
                entity.createuseraccount= createuseraccount;
                this.db.Insertable<Pro_Map>(entity).ExecuteCommand();
            }
            else
            {
                entity.Update();
                entity.mapmodelid = keyValue;
                this.db.Updateable<Pro_Map>(entity).ExecuteCommand();
            }
            result.Msg= "保存成功";
            result.Code = 200;
            result.Data = keyValue;
            return result;
        }
    }
}
