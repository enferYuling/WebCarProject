using System;
using System.Collections.Generic;
using System.Text;
using Entity;

namespace IBll
{
    public interface IMapBll
    {
        public Base_User Login(string account, string password, out string msg);
    }
}
