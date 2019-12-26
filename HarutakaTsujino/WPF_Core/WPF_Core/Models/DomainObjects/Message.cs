using System;

namespace WPF_Core.Models.DomainObjects
{
    public class Message
    {
        public Message(int id, int channelId, int userId, string text, DateTime time, int? parentMessageId)
        {
            Id = id;

            ChannelId = channelId;

            UserId = userId;

            Text = text;

            Time = time;

            ParentMessageId = parentMessageId;
        }

        public int Id { get; }

        public int ChannelId { get; }

        public int UserId { get; }
        
        public string Text { get; }

        public DateTime Time { get; }

        public int? ParentMessageId { get; }
    }
}
