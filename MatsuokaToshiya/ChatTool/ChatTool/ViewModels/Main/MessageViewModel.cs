using System;
using System.Collections.Generic;
using System.Text;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace ChatTool.ViewModels.Main
{

    class MessageViewModel
    {
        public MessageViewModel()
        {
            MessageLogService.CallMessageLog += (int eventNum) => { CallMessages(eventNum); };
        }
        public Message? SelectItem { get; set; }
        #region

        private ObservableCollection<Message> _Messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return _Messages; }
            set
            {
                if (value != this._Messages)
                    _Messages = value;
                this.SetPropertyChanged("Messages");
            }
        }
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged(string adress)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(adress));
        }

        public void SetPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CallMessages(int channelId)
        {
            var messageDao = new MessageDAO();
            messageDao.MessageList(Messages, channelId);
        }

    }
}
