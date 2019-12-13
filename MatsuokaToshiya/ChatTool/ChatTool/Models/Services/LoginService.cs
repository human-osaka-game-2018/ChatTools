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
        public static User? user;

        public static void EndLoginService()
        {
            if (user != null)
            {
                user.IsOnline = false;
                new UserDAO().Online(user);
            }
        }
        public static User[] LoadUserTable()
        {

            User[] list = new User[0];
            var userDAO = new UserDAO();
            list = userDAO.UserList(list);

            return list;
        }
        public static bool LoadUser(string mailAdress, string password)
        {
            var userDAO = new UserDAO();
            user = userDAO.User(mailAdress, password);

            if (null == user) return false;
            userDAO.Online(user);
            return user.IsOnline;
        }
    }
    
}
