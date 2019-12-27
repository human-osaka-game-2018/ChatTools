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
                new UserDAO().Online(User);
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
            User = userDAO.User(mailAdress, password);

            if (null == User) return false;
            userDAO.Online(User);
            return User.IsOnline;
        }
    }
    
}
