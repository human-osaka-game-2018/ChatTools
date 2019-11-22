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
using ChatTool.Models.Services;

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
        }
        ~Login()
        {
            LoginService.EndLoginService();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void OnClick_btnLogin(object sender, RoutedEventArgs e)
        {
            //bool succeedLogin = LoginService.LoadUser(this.txtbMailAdress.Text, this.pwbPassword.Password);
            //bool succeedLogin = LoginService.LoadUser("aaa.co.jp", "0000");

            bool succeedLogin = LoginService.LoadUser(this.txtbMailAdress.Text, "0001");
            LoginService.LoadUserTable();
            Debug.WriteLine(succeedLogin.ToString());
            if (succeedLogin) new MainWindow().Show();
        }
    }
}
