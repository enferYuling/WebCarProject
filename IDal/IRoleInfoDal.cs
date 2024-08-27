using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDal
{
    /// <summary>
    /// 角色的数据访问层接口（1、定义规范  2、多态实现）
    /// </summary>
    public interface IRoleInfoDal
    {

        bool CreateRoleInfo(RoleInfo roleInfo);
        DbSet<RoleInfo> GetRoleInfoSet();
        bool SoftDeleteRoleInfo(RoleInfo roleInfo);
        RoleInfo GetRoleInfoById(string id);
        bool UpdateRoleInfo(string id, string roleName, string description);
        bool SoftDeleteRoleInfos(List<RoleInfo> roleInfos);


    }
}
