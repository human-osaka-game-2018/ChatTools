using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.Services
{
    static class LoginService
    {
        public static User? User;

        public static void EndLoginService()
        {
            if (User != null)
            {
                User.IsOnline = false;
                UserDAO.Online(User);
            }
        }

        public static bool LoadUser(string mailAdress, string password)
        {
            User = UserDAO.User(mailAdress, password);

            if (null == User) return false;
            UserDAO.Online(User);
            return User.IsOnline;
        }
    }
    
}
