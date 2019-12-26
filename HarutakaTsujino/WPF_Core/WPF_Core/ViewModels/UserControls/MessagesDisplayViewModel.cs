using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var startChannel = 
                ChannelService.GetChannelsJoinedUser(LogInService.LogInUser)[0];

            ChannelService.SelectingChannel = startChannel;
        }

        ~MessagesDisplayViewModel()
        {
            SubscribeDisposable.Dispose();
        }

        private void SetMessagesInChannel(int channelId)
        {
            var channels =
                   ChannelService.GetChannelsJoinedUser(LogInService.LogInUser);

            foreach (var channel in channels)
            {
                if (channelId == channel.Id)
                {
                    ChannelService.SelectingChannel = channel;

                    break;
                }
            }

            var messages = 
                MessageService
                .GetMessagesInChannel(ChannelService.SelectingChannel.Id);

            Messages.Clear();

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        private IDisposable SubscribeDisposable { get; set; }
    }
}
