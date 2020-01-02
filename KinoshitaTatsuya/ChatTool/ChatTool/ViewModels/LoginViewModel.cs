using System;
using System.Collections.Generic;
using System.Text;
using ChatTool.Models.Services.LoginService;

namespace ChatTool.ViewModels
{
    public class LoginViewModel
    {
        public string MailAddress { get; set; } = "";

        public string PassWord { get; set; } = "";

        public bool CanLogin()
        {
            return LoginService.CanLogin(MailAddress,PassWord);
        }
    }
}
