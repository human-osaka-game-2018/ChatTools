using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ChatTool.Infrastructure.Database
{
    public static class Conection
    {
        public static string ConnectionString;
        public static MySqlConnection? connection { get; set; }

        static Conection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        }

        public static MySqlConnection ConnectDB()
        {
            if (null != connection) return connection;
            connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static void DisConnectDB()
        {
            if (null == connection) return;
            connection.Close();
            connection = null;
        }
    }
}
