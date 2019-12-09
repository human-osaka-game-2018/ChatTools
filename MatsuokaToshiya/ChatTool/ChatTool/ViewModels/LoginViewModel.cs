using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace ChatTool.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        static private string AdressValue = "aaa.com";
        static public string Adress
        {
            get { return AdressValue; }
            set { AdressValue = value; }
        }

        static private string PasswordValue = "0000";
        static public string Password
        {
            get { return PasswordValue; }
            set { PasswordValue = value; }
        }

        public LoginButtonCommand loginButtonCommand { get;  set; } = new LoginButtonCommand();
        ~LoginViewModel()
        {
            LoginService.EndLoginService();

        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string adress)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(adress));
            }
        }

        static public void ChangeView()
        {
            new MainWindow().Show();
        }

        public class LoginButtonCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public void Execute(object parameter)
            {
                OnClick_btnLogin();
            }

            public void OnClick_btnLogin()
            {
                bool succeedLogin = LoginService.LoadUser(LoginViewModel.Adress, LoginViewModel.Password);
                //bool succeedLogin = LoginService.LoadUser("aaa.co.jp", "0000");
                var sr = new StreamReader("../../../../longestAdress.txt", Encoding.Default);
                //bool succeedLogin = LoginService.LoadUser(sr.ReadToEnd(), "0001");
                sr.Close();
                LoginService.LoadUserTable();
                Debug.WriteLine(succeedLogin.ToString());
                if(succeedLogin) ChangeView();
            }
        }
    }
}
