using ChatTool.Command;
using ChatTool.Models.Services.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatTool.ViewModels.Main
{
    public class InputAreaViewModel : INotifyPropertyChanged
    {
        private string inputText = "";
        public string InputText 
        {
            get { return inputText; }
            set 
            { 
                inputText = value;
                SendMessageCommand.RaiseCanExecuteChanged();
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("InputText"));
            }
        }

        public DelegateCommand SendMessageCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public InputAreaViewModel()
        {
            SendMessageCommand = new DelegateCommand(SendMessage,HasInputText);
        }

        private void SendMessage()
        {
            MessageService.SendMessage(InputText);
            InputText = "";
        }

        private bool HasInputText()
        {
            if (InputText == "") return false;

            return true;
        }
    }
}
