using System;
using System.Collections.Generic;
using System.Text;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ChatTool.ViewModels.Main
{

    class MessageViewModel
    {
        public MessageViewModel()
        {
            SelectChannelService.OnChangeSelecting += (int channelId) => { ReadMessages(channelId); };
            ReadMessageService.RefleshMessageLog += () => { ReadMessages( SelectChannelService.SelectingChannelId); };
            ReplyMessageService.EraseSelectingReply += (_) => { SelectItem = null; };
            BindingOperations.EnableCollectionSynchronization(Messages, new object());
            Task.Run(PeriodicRenewAsync);
        }
        private Message? selectItem = new Message();
        public Message? SelectItem
        {
            get { return selectItem; }
            set
            {
                selectItem = value;
                ReplyMessageService.SelectingMessageChanged.Invoke(selectItem);
            }
        }
        #region

        private int MaxMessageId { get; set; } = 0;

        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return messages; }
            set
            {
                if (value != this.messages)
                    messages = value;
                this.SetPropertyChanged("Messages");
            }
        }
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ReadMessages( int channelId)
        {
            messages.Clear();
            MessageDAO.MessageList(messages, channelId);
            MaxMessageId = MessageDAO.GetMaxMessageIdInChannel(channelId);
        }

        #region
        async private Task PeriodicRenewAsync()
        {
            while (true)
            {
                await Task.Delay(5000);
                if (MessageDAO.GetMaxMessageIdInChannel(SelectChannelService.SelectingChannelId) != MaxMessageId)
                {
                    ReadMessages(SelectChannelService.SelectingChannelId);
                }
            }
        }
        #endregion
    }
}
