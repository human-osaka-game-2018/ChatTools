using ChatTool.Infrastructure.Database.MessageDAO;
using ChatTool.Models.DomainObjects.Message;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services.Main
{
    public static class MessageService
    {
        public static void SendMessage(string? text)
        {
            var message = new Message();

            var currentChannel = ChannelService.CurrentChannel;

            if (currentChannel == null && text == "") return;

            message.ChannelId = currentChannel.Id;
            message.User = LoginService.LoginService.LoginUser;
            message.Text = text;
            message.Time = DateTime.Now;
            message.ParentMessageId = 1;
            message.DisplaysToChannel = true;

            MessageDAO.SendMessage(message);

            MessageSent?.Invoke();
        }

        public static ObservableCollection<Message> GetMessages()
        {
            return MessageDAO.GetMessages(ChannelService.CurrentChannel.Id);
        }

        public static event Action? MessageSent;
    }
}
