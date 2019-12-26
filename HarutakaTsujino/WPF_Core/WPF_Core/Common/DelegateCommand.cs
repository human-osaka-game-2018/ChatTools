using System;
using System.Windows.Input;

namespace WPF_Core.Common
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public DelegateCommand(Action<object?> onExecute)
        {
            this.onExecute = onExecute;
        }

        public DelegateCommand(Action<object?> onExecute, Func<object?, bool> onCanExecute)
        {
            this.onExecute = onExecute;
            OnCanExecute = onCanExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (OnCanExecute is null)
            {
                return true;
            }
            else
            {
                return OnCanExecute(parameter);
            }
        }

        public void Execute(object? parameter)
        {
            onExecute?.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private readonly Action<object?>? onExecute;

        private Func<object?, bool>? onCanExecute;
        private Func<object?, bool>? OnCanExecute
        {
            get => onCanExecute;
            set
            {
                onCanExecute = value;

                RaiseCanExecuteChanged();
            }
        }
    }
}
