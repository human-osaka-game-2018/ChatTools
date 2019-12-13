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
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string AddressValue = "aaa.com";
        public string Address
        {
            get { return AddressValue; }
            set {
                AddressValue = value;
                LoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        private string PasswordValue = "0000";
        public string Password
        {
            get { return PasswordValue; }
            set { 
                PasswordValue = value;
                LoginButtonCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginButtonCommand { get; }
        public LoginViewModel()
        {
            LoginButtonCommand = new DelegateCommand(OnClick_btnLogin, CanExecute);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string adress)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(adress));
        }
        public event EventHandler? LoginSucceed;
        public event EventHandler? LoginFailed;
        //TODO:staticの位置をpublicの後ろ等にする
        public static void ChangeView()
        {
            new MainWindow().Show();
        }

        private void OnClick_btnLogin()
        {
            bool succeedLogin = LoginService.LoadUser(Address, Password);
            //bool succeedLogin = LoginService.LoadUser("aaa.co.jp", "0000");
            //var sr = new StreamReader("../../../../longestAddress.txt", Encoding.Default);
            //bool succeedLogin = LoginService.LoadUser(sr.ReadToEnd(), "0001");
            //sr.Close();
            //LoginService.LoadUserTable();
            Debug.WriteLine(succeedLogin.ToString());
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
