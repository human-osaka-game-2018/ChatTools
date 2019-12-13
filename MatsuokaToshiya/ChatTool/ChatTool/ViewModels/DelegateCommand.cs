using System;
using System.Windows.Input;

namespace ChatTool.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private static readonly Action EmptyExecute = () => { };
        private static readonly Func<bool> EmptyCanExecute = () => true;

        private Action execute;
        private Func<bool> canExecute;

        public DelegateCommand(Action execute, Func<bool>? canExecute = null)
        {
            this.execute = execute ?? EmptyExecute;
            this.canExecute = canExecute ?? EmptyCanExecute;
        }

        public void Execute()
        {
            this.execute();
        }

        public bool CanExecute()
        {
            return this.canExecute();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        public event EventHandler? CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }
    }
}
