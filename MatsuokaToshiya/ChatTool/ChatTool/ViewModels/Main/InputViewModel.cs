using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ChatTool.ViewModels.Main
{
    class InputViewModel : INotifyPropertyChanged
    {
        private string inputText = "";
        public string InputText {
            get { return inputText; }
            set {
                inputText = value;
                SendButtonCommand.RaiseCanExecuteChanged();
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("InputText"));
            }
        }
        public InputViewModel()
        {
            SendButtonCommand = new DelegateCommand(SendMessage, CanExecute);
            EraseButtonCommand  = new DelegateCommand(EraseReply);
            ReplyMessageService.SelectingMessageChanged += (message) => { ReplyMessage = message; };
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
            if (!CanExecute()) return;
            if (ReplyMessage == null)
            {
                sendMessageService.SendMessage(InputText);
            }
            else
            {
                sendMessageService.SendReplyMessage(InputText, ReplyMessage.Id);
            }
            InputText = string.Empty;
            EraseReply();
        }

        private bool CanExecute()
        {
            if ("" == InputText) return false;
            return true;
        }
        #region
        private static string repliedUserName = "";
        public string RepliedUserName {
            get
            {
                return repliedUserName;
            }
            set
            {
                repliedUserName = value;
                SetPropertyChanged("RepliedUserName");
            }
        }
        private static string repliedMessage = "";
        public string RepliedMessage
        {
            get
            {
                return repliedMessage;
            }
            set
            {
                repliedMessage = value;
                SetPropertyChanged("RepliedMessage");
            }
        }
        private static DateTime repliedMessageTime = DateTime.Now;
        public DateTime RepliedMessageTime
        {
            get
            {
                return repliedMessageTime;
            }
            set
            {
                repliedMessageTime = value;
                SetPropertyChanged("RepliedMessageTime");
            }

        }
        #endregion
        private Message? replyMessage = null;
        public Message? ReplyMessage
        {
            get
            {
                return replyMessage;
            }
            set
            {
                replyMessage = value;
                if (replyMessage == null) return;
                RepliedUserName = replyMessage.UserName;
                RepliedMessage = replyMessage.Text;
                RepliedMessageTime = replyMessage.Time;
            }
        }

        public DelegateCommand EraseButtonCommand { get; }
        public void EraseReply()
        {
            if (replyMessage == null) return;
            ReplyMessageService.EraseSelectingReply.Invoke(0);
            RepliedUserName = "";
            RepliedMessage = "";
            RepliedMessageTime = DateTime.MinValue;
            ReplyMessage = null;
        }
    }
}
