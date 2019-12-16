﻿using System;
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
using WPF_Core.Models.Services;
using WPF_Core.ViewModels;

namespace WPF_Core.Views
{
    /// <summary>
    /// LogInView.xaml の相互作用ロジック
    /// </summary>
    public partial class LogInView : Window
    {
        public LogInView()
        {
            InitializeComponent();

            SubscribeDisposable = LogInViewModel.OnLogInSucceededAsObservable
                .Subscribe(_ => OpenEditorView());

            DataContext = LogInViewModel;

#if DEBUG
            LogInViewModel.MailAddress = "mail@ress";
            passBoxPassword.Password = "0000";
#endif
        }

        ~LogInView()
        {
            SubscribeDisposable.Dispose();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            LogInViewModel.Password = passBoxPassword.Password;
        }

        private void OpenEditorView()
        {
            var editorView = new EditorView();

            editorView.ShowDialog();
        }

        private IDisposable SubscribeDisposable { get; set; }

        private LogInViewModel LogInViewModel { get; set; } = new LogInViewModel();
    }
}
