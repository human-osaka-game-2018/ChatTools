using ChatTool.Models.Services.LoginService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatTool.ViewModels.Main
{
    public class MainViewModel : INotifyPropertyChanged

    {
        private string? iconPath = "";
        public string? IconPath
        {
            get { return iconPath; }
            set 
            {
                iconPath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IconPath"));
            }
        }

        private string? userName = "";
        public string? UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        public MainViewModel()
        {
            var user = LoginService.LoginUser;

            UserName = user.Name;
            IconPath = user.IconPath;
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
