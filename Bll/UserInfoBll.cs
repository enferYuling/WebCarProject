using System;
using System.Collections.Generic;
using System.Text;
using CarProject.Common;
using Common;
using Entity;
using IBll;
using SqlSugar;

namespace Bll
{
    public class UserInfoBll : IUserInfoBll
    {
        public ISqlSugarClient db;
        public UserInfoBll(ISqlSugarClient datadb) 
        {
            this.db = datadb;
        }
       public Base_User  Login(string account, string password, out string msg)
        {
            
            if (string.IsNullOrEmpty(account))
            {
                msg="请输入账号";
                
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                msg = "请输入密码";
              
                return null;
            }

            var base_user = this.db.Queryable<Base_User>().Where(a => a.account == account).First();
            var pwd = MD5Help.GetMD5Hash(password + "CarProject");
            if (base_user == null)
            {
                msg = "该用户不存在，请在客户端注册";
                return null;
            }
            if (pwd != base_user.password)
            {
               msg="密码错误，请重新输入";
                
                return null;
            }
            
            base_user.Loginnumber += 1;
           
            base_user.logintime = DateTime.Now;
            this.db.Updateable(base_user).UpdateColumns(it => new { it.logintime, it.loginmonth, it.Loginnumber, it.monthnumber }).ExecuteCommand();
            msg = "登陆成功";
            return base_user;
        }
    }
}
