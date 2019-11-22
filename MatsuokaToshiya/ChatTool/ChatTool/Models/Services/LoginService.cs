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
        static User user;
        static private MySqlConnection connection;
        public static void StartLoginService()
        {
            connection = Conection.ConnectDB();
        }
        public static void EndLoginService()
        {
            user.IsOnline = false;
            new UserDAO().Online(user, Conection.ConnectDB());
            Conection.DisConnectDB();
        }
        public static User[] LoadUserTable()
        {
            connection = Conection.ConnectDB();

            User[] list = new User[0];
            var userDAO = new UserDAO();
            list = userDAO.UserList(list, connection);
            Conection.DisConnectDB();

            return list;
        }
        public static bool LoadUser(string mailAdress, string password)
        {
            connection = Conection.ConnectDB();
            var userDAO = new UserDAO();
            var aaa = mailAdress;
            user = userDAO.User(aaa, password, connection);

            Conection.DisConnectDB();

            userDAO.Online(user, Conection.ConnectDB());
            Conection.DisConnectDB();
            return user.IsOnline;
        }
    }
    
}
