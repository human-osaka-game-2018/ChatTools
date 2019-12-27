using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ChatTool.ViewModels.Main
{
    class InputViewModel
    {
        private string _InputText = "";
        public string InputText {
            get { return _InputText; }
            set {
                _InputText = value;
                SendButtonCommand.RaiseCanExecuteChanged();
            }
        }
        public InputViewModel()
        {
            SendButtonCommand = new DelegateCommand(SendMessage, CanExecute);
        }
        public DelegateCommand SendButtonCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void SendMessage()
        {
            var sendMessageService = new SendMessageService();
            sendMessageService.SendMessage(InputText);
        }
        private bool CanExecute()
        {
            if ("" == InputText) return false;
            return true;
        }

    }
}
