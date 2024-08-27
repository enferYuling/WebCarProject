 
using SqlSugar; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;//读取配置文件类

namespace Common
{
    public class SqlSugarConfig
    {
        ////private static readonly string connectionString = "Data Source=NXCL2024;Database=CarProject;User Id=sa;Password=car2024@;";
        private static readonly string connectionString = "Data Source=.;Database=CarProject;User Id=sa;Password=123456;";

       
        public static SqlSugarClient GetInstance()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            return db;
        }
    }
}
