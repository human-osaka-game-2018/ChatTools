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
            var startChannel = 
                ChannelDataProviderService.GetJoinedUser(LogInService.LogInUser.Id)[0];

            ChannelDataProviderService.SelectingChannel = startChannel;

            var messages = ChannelDataProviderService.GetMessagesInChannel(startChannel.Id);

            foreach (var message in messages)
            {
                Messages.Add(message);
            }

            subscribeDisposable =
                ChannelDataProviderService.OnSelectingChangedAsObservable
                .Subscribe(SetMessagesInChannel);
        }

        ~MessagesDisplayViewModel()
        {
            subscribeDisposable.Dispose();
        }

        private void SetMessagesInChannel(int channelId)
        {
            var channels =
                   ChannelDataProviderService.GetJoinedUser(LogInService.LogInUser.Id);

            foreach (var channel in channels)
            {
                if (channelId == channel.Id)
                {
                    ChannelDataProviderService.SelectingChannel = channel;

                    break;
                }
            }

            var messages = 
                ChannelDataProviderService
                .GetMessagesInChannel(ChannelDataProviderService.SelectingChannel.Id);

            Messages.Clear();

            foreach (var message in messages)
            {
                Messages.Add(message);
            }
        }

        private IDisposable subscribeDisposable;
    }
}
