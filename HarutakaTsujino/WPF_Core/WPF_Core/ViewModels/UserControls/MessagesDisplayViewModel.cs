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
                .Subscribe(ch => 
                {
                    if (ch is null) return;

                    SetMessagesInChannel(ch);
                });
        }

        ~MessagesDisplayViewModel()
        {
            SubscribeDisposable.Dispose();
        }

        private void SetMessagesInChannel(Channel channel)
        {
            var messages = MessageService.GetMessagesInChannel(channel);

            Messages.Clear();

            foreach (var message in messages)
            {
                // Addしているから今は表示されている
                Messages.Add(message);
            }
        }

        private IDisposable SubscribeDisposable { get; set; }
    }
}
