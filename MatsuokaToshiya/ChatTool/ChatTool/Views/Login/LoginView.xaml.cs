using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using ChatTool.Views.Main;

namespace ChatTool.Views
{
    /// <summary>
    /// Login.xaml の相互作用ロジック
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            viewModel.LoginSucceed += (_, __) => { new MainView().Show(); };
            viewModel.LoginFailed += (_, __) => { MessageBox.Show("ログイン情報に間違いがあります。");
        };
            this.DataContext = viewModel;
            
        }
        ~Login()
        {
        }


        private void pwbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Password = pwbPassword.Password;
        }

        private void pwbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel)DataContext).Password = pwbPassword.Password;
        }
    }
}
