using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class MessagesDisplayViewModel
    {
        public ObservableCollection<Message> Messages { get; private set; } =
            new ObservableCollection<Message>();

        public MessagesDisplayViewModel()
        {
            SubscribeDisposable =
                ChannelService.OnSelectingChangedAsObservable
                .Subscribe(SetMessagesInChannel);

            if (LogInService.LogInUser is null) return;

            var startChannel = 
                ChannelService.GetChannelsJoinedUser(LogInService.LogInUser).ToList()[0];

            ChannelService.SelectingChannel = startChannel;
        }

        ~MessagesDisplayViewModel()
        {
            SubscribeDisposable.Dispose();
        }

        private void SetMessagesInChannel(int channelId)
        {
            if (LogInService.LogInUser is null) return; 

            var channels =
                   ChannelService.GetChannelsJoinedUser(LogInService.LogInUser);

            if (channels is null) return;

            var channel = channels.SingleOrDefault(x => channelId == x.Id);

            if (channel is null) return;

            ChannelService.SelectingChannel = channel;

            var messages = 
                MessageService
                .GetMessagesInChannel(ChannelService.SelectingChannel);

            Messages.Clear();

            if (messages is null) return;

            foreach (var message in messages)
            {
                // Addしているから今は表示されている
                Messages.Add(message);
            }
        }

        private IDisposable SubscribeDisposable { get; set; }
    }
}
