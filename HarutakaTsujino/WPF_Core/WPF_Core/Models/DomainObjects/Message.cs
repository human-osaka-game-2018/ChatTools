using System;

namespace WPF_Core.Models.DomainObjects
{
    public class Message
    {
        public Message(int id, Channel channel, User user, string text, DateTime time, Message? parentMessage = null)
        {
            Id = id;

            Channel = channel;

            User = user;

            Text = text;

            Time = time;

            ParentMessage = parentMessage;
        }

        public int Id { get; }

        public Channel Channel { get; }

        public User User { get; }

        public string UserName => User.Name;
        
        public string Text { get; }

        public DateTime Time { get; }

        public Message? ParentMessage { get; }
    }
}
