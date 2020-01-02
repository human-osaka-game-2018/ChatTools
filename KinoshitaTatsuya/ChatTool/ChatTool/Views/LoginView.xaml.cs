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
using ChatTool.ViewModels;

namespace ChatTool.Views
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            loginViewModel.PassWord = PwbPassword.Password;

            if (loginViewModel.CanLogin())
            {
                var nextWindow = new NextWindow();
                nextWindow.Show();                
            }
            else
            {
                MessageBox.Show("メールアドレスかパスワードが間違っています。");
            }            
        }

        private LoginViewModel loginViewModel = new LoginViewModel();
    }
}
