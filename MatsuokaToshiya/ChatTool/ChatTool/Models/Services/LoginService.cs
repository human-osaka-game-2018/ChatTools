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
        static User? user;
        public static void StartLoginService()
        {
            Conection.ConnectDB();
        }
        public static void EndLoginService()
        {
            if (user != null)
            {
                user.IsOnline = false;
                new UserDAO().Online(user, Conection.ConnectDB());
            }
            Conection.DisConnectDB();
        }
        public static User[] LoadUserTable()
        {
            Conection.ConnectDB();

            User[] list = new User[0];
            var userDAO = new UserDAO();
            list = userDAO.UserList(list, Conection.connection);
            Conection.DisConnectDB();

            return list;
        }
        public static bool LoadUser(string mailAdress, string password)
        {
            Conection.ConnectDB();
            var userDAO = new UserDAO();
            var aaa = mailAdress;
            user = userDAO.User(aaa, password, Conection.connection);

            Conection.DisConnectDB();
            if (null == user) return false;
            userDAO.Online(user, Conection.ConnectDB());
            Conection.DisConnectDB();
            return user.IsOnline;
        }
    }
    
}
