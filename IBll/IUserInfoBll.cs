using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace IBll
{
    public interface IUserInfoBll
    {
        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <param name="ischek">是否勾选</param>
        /// <returns></returns>
        public Base_User Login(string account, string password,out string msg);
    }
}
