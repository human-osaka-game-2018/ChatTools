using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class MessagesDisplayViewModel : BindableBase
    {
        public ObservableCollection<Message?> Messages 
        {
            get => messages;
            set => SetValue(ref messages, value);
        }

        public MessagesDisplayViewModel()
        {
            subscribeDisposables.Add(
                ChannelService.OnSelectingChangedAsObservable
                .Subscribe(ch => 
                {
                    if (ch is null) return;

                    SetMessagesInChannel(ch);
                }));

            subscribeDisposables.Add(
                MessageService.OnMessagePostedAsObservable
                .Subscribe(_ =>
                {
                    SetMessagesInChannel(ChannelService.SelectingChannel!);
                }));
        }

        ~MessagesDisplayViewModel()
        {
            subscribeDisposables.ForEach(diposable => diposable.Dispose());
        }

        private void SetMessagesInChannel(Channel channel)
        {
            var latestMessages = MessageService.Get(channel);

            Messages.Clear();
            Messages = new ObservableCollection<Message?>(latestMessages);
        }

        private ObservableCollection<Message?> messages = new ObservableCollection<Message?>();

        private readonly List<IDisposable> subscribeDisposables = new List<IDisposable>();
    }
}
