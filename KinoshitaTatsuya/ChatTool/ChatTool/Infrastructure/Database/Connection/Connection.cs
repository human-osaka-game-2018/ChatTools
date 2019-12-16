using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ChatTool.Infrastructure.Database.Connection
{
    public static class Connection
    {
        private static string? connectionStr = null;

        static Connection()
        {
            connectionStr = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        }

        public static MySqlConnection OpenDB()
        {
            var con = new MySqlConnection(connectionStr);
            con.Open();

            return con;
        }
    }
}
