using System;
using System.Collections.Generic;
using System.Text;
using ChatTool.Infrastructure.Database.UserDAO;
using ChatTool.Models.DomainObjects.User;

namespace ChatTool.Models.Services.LoginService
{
    static public class LoginService
    {
        static public User LoginUser { get; set; } = new User();
        static public bool CanLogin(string mailAddress, string password)
        {            
            var LoginInfo = UserDAO.GetLoginInfos();

            if (!LoginInfo.ContainsKey(mailAddress)) return false;

            if (LoginInfo[mailAddress].Item1 != password) return false;

            LoginUser = UserDAO.GetUser(LoginInfo[mailAddress].Item2);

            UserDAO.UpdateOnline(LoginUser.Id, true);

            return true;
        }
    }
}
