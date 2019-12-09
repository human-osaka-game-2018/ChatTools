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
using ChatTool.ViewModels;

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
            this.DataContext = viewModel;
            
        }
        ~Login()
        {
        }


        private void pwbPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            LoginViewModel.Password = pwbPassword.Password;
        }
    }
}
