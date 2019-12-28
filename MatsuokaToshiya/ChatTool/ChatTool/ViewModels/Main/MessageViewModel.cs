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
            SelectChannelService.CallMessageLog += (int channelId) => { CallMessages(channelId); };
            CallMessageService.RefleshMessageLog += () => { CallMessageService.CallMessages(Messages, SelectChannelService.SelectingChannelId); };
        }
        private Message _SelectItem = new Message();
        public Message? SelectItem
        {
            get { return _SelectItem; }
            set
            {
                if (null == value) return;
                _SelectItem = value;
                ReplyMessageService.ChangedSelectingMessage.Invoke(_SelectItem);
            }
        }
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
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void CallMessages(int channelId)
        {
            CallMessageService.CallMessages(Messages, channelId);
        }

    }
}
