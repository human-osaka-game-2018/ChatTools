using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WPF_Core.Infrastructure.Database
{
    public static class Connection
    {
        static Connection()
        {
            connectionString = FetchConnectionString();
        }

        public static MySqlConnection Connect()
        {
            var mySqlConnection = new MySqlConnection(connectionString);

            return mySqlConnection;
        }

        private static string FetchConnectionString()
        {
            var runMode = ConfigurationManager.AppSettings["RunMode"];

            return ConfigurationManager.ConnectionStrings["Default." + runMode].ConnectionString;
        }

        private static readonly string connectionString;
    }
}
