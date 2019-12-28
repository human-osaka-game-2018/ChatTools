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
            EraseButtonCommand  = new DelegateCommand(EraseReply);
            ReplyMessageService.ChangedSelectingMessage = (message) => { ReplyMessage = message; };
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
            if (ReplyMessage == null)
            {
                sendMessageService.SendMessage(InputText);
                return;
            }
            sendMessageService.SendReplyMessage(InputText, ReplyMessage.Id);
        }
        private bool CanExecute()
        {
            if ("" == InputText) return false;
            return true;
        }
        #region
        private static string _RepliedUserName = "";
        public string RepliedUserName {
            get
            {
                return _RepliedUserName;
            }
            set
            {
                _RepliedUserName = value;
                SetPropertyChanged("RepliedUserName");
            }
        }
        private static string _RepliedMessage = "";
        public string RepliedMessage
        {
            get
            {
                return _RepliedMessage;
            }
            set
            {
                _RepliedMessage = value;
                SetPropertyChanged("RepliedMessage");
            }
        }
        private static DateTime _RepliedMessageTime = DateTime.Now;
        public DateTime RepliedMessageTime
        {
            get
            {
                return _RepliedMessageTime;
            }
            set
            {
                _RepliedMessageTime = value;
                SetPropertyChanged("RepliedMessageTime");
            }

        }
        #endregion
        private Message? _ReplyMessage = null;
        public Message? ReplyMessage
        {
            get
            {
                return _ReplyMessage;
            }
            set
            {
                _ReplyMessage = value;
                if (_ReplyMessage == null) return;
                RepliedUserName = _ReplyMessage.UserName;
                RepliedMessage = _ReplyMessage.Text;
                RepliedMessageTime = _ReplyMessage.Time;
            }
        }
        public DelegateCommand EraseButtonCommand { get; }
        public void EraseReply()
        {
            RepliedUserName = "";
            RepliedMessage = "";
            RepliedMessageTime = DateTime.MinValue;
            ReplyMessage = null;

        }
    }
}
