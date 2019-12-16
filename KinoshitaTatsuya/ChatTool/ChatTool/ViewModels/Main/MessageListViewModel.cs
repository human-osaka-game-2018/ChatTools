using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using ChatTool.Infrastructure.Database.MessageDAO;
using ChatTool.Models.DomainObjects.Message;
using ChatTool.Models.Services.Main;

namespace ChatTool.ViewModels.Main
{
    public class MessageListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Message> messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages 
        {
            get { return messages; }
            set
            {
                messages = value;                
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Messages"));                               
            }
        }        

        public event PropertyChangedEventHandler? PropertyChanged;

        public MessageListViewModel()
        {            
            ChannelService.CurrentChannelChanged += () =>
            {               
                Messages = MessageDAO.GetMessages(ChannelService.CurrentChannel.Id);
            };
        }
    }
}
