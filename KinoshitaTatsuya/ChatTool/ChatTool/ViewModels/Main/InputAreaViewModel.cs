using ChatTool.Bases;
using ChatTool.Command;
using ChatTool.Models.Services.Main;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatTool.ViewModels.Main
{
    public class InputAreaViewModel : BindableBase
    {       
        private string inputText = "";
        public string InputText 
        {
            get { return inputText; }
            set 
            {                   
                SetProperty(ref inputText, value);
                SendMessageCommand.RaiseCanExecuteChanged();
            }
        }        

        public DelegateCommand SendMessageCommand { get; set; }       

        public InputAreaViewModel()
        {
            SendMessageCommand = new DelegateCommand(SendMessage,HasInputText);
        }

        private void SendMessage()
        {
            if (!CanSendMessage) return;

            MessageService.SendMessage(InputText);
            InputText = "";
        }

        private bool HasInputText()
        {
            return this.InputText.Length > 0;
        }

        private bool CanSendMessage => inputText.Length < MaxStringLength;

        private const int MaxStringLength = 140;
    }
}
