using System;
using System.Windows;
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

            subscribeDisposable = logInViewModel.OnLogInSucceededAsObservable
                .Subscribe(_ => OpenEditorView());

            DataContext = logInViewModel;

#if DEBUG
            logInViewModel.MailAddress = "mail@ress";
            passBoxPassword.Password = "0000";
#endif
        }

        ~LogInView()
        {
            subscribeDisposable.Dispose();
        }

        private void BtnLogIn_Click(object sender, RoutedEventArgs e)
        {
            logInViewModel.Password = passBoxPassword.Password;
        }

        private void OpenEditorView()
        {
            var editorView = new EditorView();

            editorView.ShowDialog();
        }

        private readonly IDisposable subscribeDisposable;

        private readonly LogInViewModel logInViewModel = new LogInViewModel();
    }
}
