using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Windows.Input;
using WPF_Core.Common;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels
{
    public class LogInViewModel
    {
        public string MailAddress { get; set; }

        public string Password { get; set; }
     
        public ICommand LogInCommand { get; set; }

        public IObservable<Unit> OnLogInSucceededAsObservable => onLogInSucceededAsSubject;

        public IObservable<Unit> OnLogInFailedAsObservable => onLogInSucceededAsSubject;

        public LogInViewModel()
        {
            LogInCommand = new DelegateCommand(LogIn);
        }

        private void LogIn(object _)
        {
            var logInResult = LogInService.LogIn(MailAddress, Password);

            Password = "";

            if (logInResult)
            {
                onLogInSucceededAsSubject.OnNext(Unit.Default);
            }
            else
            {
                onLogInFailedAsSubject.OnNext(Unit.Default);
            }
        }

        private readonly Subject<Unit> onLogInSucceededAsSubject = new Subject<Unit>();

        private readonly Subject<Unit> onLogInFailedAsSubject = new Subject<Unit>();
    }
}
