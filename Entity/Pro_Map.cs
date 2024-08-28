using System;
using System.Linq;
using System.Text;

using SqlSugar;
namespace Entity
{
    ///<summary>
    ///地图建模
    ///</summary>
    public partial class Pro_Map
    {
        #region 实体配置
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsIgnore=false, IsPrimaryKey = true)]
           public string mapmodelid {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public string fileaddress {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:False
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public string mapName {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public DateTime? filetime {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public DateTime? createdate {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public DateTime? updatedate {get;set;}

           /// <summary>
           /// Desc:
           /// Default:
           /// Nullable:True
           /// </summary>           
           [SugarColumn(IsIgnore=false)]
           public string createuseraccount {get;set;}
        /// <summary>
        /// Desc:是否有效(1-有效,0-无效)
        /// Default:
        /// Nullable:True
        /// </summary>           
        [SugarColumn(IsIgnore=false)]
           public int Enabled { get;set;}
        #endregion
        #region 扩展操作
        public void Create()
        {
            this.mapmodelid=Guid.NewGuid().ToString().Replace("-","");
            this.createdate=DateTime.Now;
            this.Enabled = 1;
        }
        public void Update()
        {
           
            this.updatedate = DateTime.Now;
        }
        public void Delete()
        {

            this.Enabled = 0;
        }
        #endregion
    }
}
