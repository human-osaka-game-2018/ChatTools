using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ChatTool.ViewModels.Login;

namespace ChatTool.Views.Login
{
    /// <summary>
    /// LoginView.xaml の相互作用ロジック
    /// </summary>
    public partial class LoginView : Window
    {        
        public LoginView()
        {
            InitializeComponent();
            DataContext = loginViewModel;

            loginViewModel.LoginSuccesEvent += new EventHandler((_,__) => new Main.MainView().Show());
            loginViewModel.LoginFailedEvent += new EventHandler((_,__) => MessageBox.Show("メールアドレスかパスワードが間違っています"));
        }   

        private void pwbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            loginViewModel.PassWord = pwbPassword.Password;            
        }             

        private LoginViewModel loginViewModel = new LoginViewModel();        
    }
}