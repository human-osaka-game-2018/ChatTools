using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ChatTool.ViewModels.Login
{
    public class LoginViewModel 
    {
        private string address = "";
        public string Address
        {
            get { return address; }
            set {
                address = value;
                LoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        private string password = "";
        public string Password
        {
            get { return password; }
            set { 
                password = value;
                LoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginButtonCommand { get; }
        public LoginViewModel()
        {
            LoginButtonCommand = new DelegateCommand(OnClick_btnLogin, CanExecute);
        }

        public event EventHandler? LoginSucceed;
        public event EventHandler? LoginFailed;

        public static void ChangeView()
        {
            new MainWindow().Show();
        }

        private void OnClick_btnLogin()
        {
            bool succeedLogin = LoginService.LoadUser(Address, Password);
            if (succeedLogin)
            {
                LoginSucceed?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                LoginFailed?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool CanExecute()
        {
            if ("" == Address|| "" == Password) return false;
            return true;
        }
    }
}
