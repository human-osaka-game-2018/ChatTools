using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ChatTool.Bases;
using ChatTool.Models.DomainObjects.Message;
using ChatTool.Models.Services.Main;

namespace ChatTool.ViewModels.Main
{
    public class MessageListViewModel : BindableBase
    {
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages 
        {
            get { return messages; }
            set { SetProperty(ref messages, value); }            
        }

        private Message selectedMessage = new Message();
        public Message SelectedMessage 
        {
            get { return selectedMessage; }
            set 
            {
                SetProperty(ref selectedMessage, value);
                MessageService.ReplyMessage = selectedMessage;
            }
        }                

        public MessageListViewModel()
        {            
            ChannelService.CurrentChannelChanged += () =>
            {               
                Messages = MessageService.GetMessages();
            };

            MessageService.MessageSent += () =>
            {
                Messages = MessageService.GetMessages();
            };

            BindingOperations.EnableCollectionSynchronization(this.Messages, new object());
            const int DelayTime_ms = 30000;
            Task.Run(async () =>
           {
               while(true)
               {
                   Messages = MessageService.GetMessages();

                   await Task.Delay(DelayTime_ms);
               }
           });
        }        
    }
}
