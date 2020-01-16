using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
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

        public IObservable<Unit> OnMessagePostedAsObservable => MessageService.OnMessagePostedAsObservable;

        public MessagesDisplayViewModel()
        {
            subscription = MessageService.OnMessagesFetchedAsObservable
                .Subscribe(messages => SetMessagesInChannel(messages));
        }

        ~MessagesDisplayViewModel()
        {
            subscription!.Dispose();
        }

        private void SetMessagesInChannel(IEnumerable<Message?> messages)
        {
            Messages.Clear();
            Messages = new ObservableCollection<Message?>(messages);
        }

        private ObservableCollection<Message?> messages = new ObservableCollection<Message?>();

        private readonly IDisposable? subscription = null;
    }
}
