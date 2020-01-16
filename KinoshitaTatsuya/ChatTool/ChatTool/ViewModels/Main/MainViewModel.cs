using ChatTool.Bases;
using ChatTool.Models.Services.LoginService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatTool.ViewModels.Main
{
    public class MainViewModel : BindableBase

    {
        private string? iconPath = "";
        public string? IconPath
        {
            get { return iconPath; }
            set { SetProperty(ref iconPath, value); }
        }

        private string? userName = "";
        public string? UserName
        {
            get { return userName; }
            set { SetProperty(ref userName, value); }            
        }

        public MainViewModel()
        {
            var user = LoginService.LoginUser;

            UserName = user.Name;
            IconPath = user.IconPath;
            
        }        
    }
}
