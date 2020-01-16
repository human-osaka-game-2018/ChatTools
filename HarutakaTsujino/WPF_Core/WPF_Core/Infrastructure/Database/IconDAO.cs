using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.IO;

namespace WPF_Core.Infrastructure.Database
{
    public static class IconDAO
    {
        public static string GetPath(int id)
        {
            var iconPath = Path.GetFullPath(@ConfigurationManager.AppSettings[$"Icon_{id}"]);

            if (string.IsNullOrEmpty(iconPath))
            {
                iconPath = Path.GetFullPath(@ConfigurationManager.AppSettings[$"Icon_0"]);
            }

            return iconPath;
        }
    }
}
