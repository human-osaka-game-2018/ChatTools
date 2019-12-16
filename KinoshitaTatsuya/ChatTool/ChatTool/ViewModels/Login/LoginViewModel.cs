using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ChatTool.Command;
using ChatTool.Models.Services.LoginService;
using ChatTool.Views;

namespace ChatTool.ViewModels.Login
{    

    public class LoginViewModel
    {
        private string mailAddress = "";
        public string MailAddress 
        {
            get { return mailAddress; }
            set
            {
                mailAddress = value;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string password = "";
        public string PassWord
        {
            get { return password; }
            set
            {
                password = value;
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginCommand { get; set; }
        public event EventHandler? LoginSuccesEvent;
        public event EventHandler? LoginFailedEvent;
        
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(Login,HasInputInfo);
        }

        private void Login()
        {
            if(LoginService.CanLogin(MailAddress,PassWord))
            {
                LoginSuccesEvent?.Invoke(this,EventArgs.Empty);                
            }
            else
            {
                LoginFailedEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool HasInputInfo()
        {
            if (MailAddress == "" || PassWord == "") return false;

            return true;
        }
    }
}
