using System;
using System.Collections.Generic;
using System.Text;

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

        public int Id { get; private set; }

        public int ChannelId { get; private set; }

        public int UserId { get; private set; }
        
        public string Text { get; private set; }

        public DateTime Time { get; private set; }

        public int? ParentMessageId { get; private set; }
    }
}
