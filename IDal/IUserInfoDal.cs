using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace IDal
{
    /// <summary>
    /// 用户的数据访问接口层
    /// </summary>
    public interface IUserInfoDal
    {
        bool CreateUserInfo(UserInfo userInfo);
        bool UpdateUserInfo(string id, string userName, int sex, string email, string phoneNum, string departmentId);
        UserInfo GetUserInfoById(string id);
        DbSet<UserInfo> GetUserInfoSet();
        bool SoftDeleteUserInfo(string id);


    }
}
