using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ChatTool.Command
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private Action execute;
        private Func<bool>? canExecute;

        public DelegateCommand(Action Execute)
        {
            execute = Execute;
            canExecute = null;
        }

        public DelegateCommand(Action Execute, Func<bool>? CanExecute = null)
        {
            execute = Execute;
            canExecute = CanExecute;
        }       

        public void Execute()
        {
            execute();
        }

        public void Execute(object param)
        {
            execute();
        }

        public bool CanExecute()
        {
            if (canExecute == null) return true;

            return canExecute();
        }

        public bool CanExecute(object param)
        {
            if (canExecute == null) return true;

            return canExecute();
        }

        public void RaiseCanExecuteChanged()
        {            
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);       
        }
    }
}
