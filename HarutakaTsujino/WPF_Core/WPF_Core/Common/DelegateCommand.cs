using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WPF_Core.Common
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> onExecute)
        {
            OnExecute = onExecute;
        }

        public DelegateCommand(Action<object> onExecute, Func<object, bool> onCanExecute)
        {
            OnExecute = onExecute;
            OnCanExecute = onCanExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (OnCanExecute == null)
            {
                return true;
            }
            else
            {
                return OnCanExecute(parameter);
            }
        }

        public void Execute(object parameter)
        {
            OnExecute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private Action<object> OnExecute { get; set; }

        private Func<object, bool> onCanExecute;
        private Func<object, bool> OnCanExecute
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
