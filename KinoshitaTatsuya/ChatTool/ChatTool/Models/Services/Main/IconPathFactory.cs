using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ChatTool.Models.Services.Main
{
    public static class IconPathFactory
    {
        public static string Create(int Id)
        {
            return ConfigurationManager.AppSettings[Id - 1];
        }
    }
}
