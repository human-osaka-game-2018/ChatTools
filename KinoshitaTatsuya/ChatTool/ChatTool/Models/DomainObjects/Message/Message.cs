using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects.Message
{
    public class Message
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public User.User? User { get; set; }
        public string? Text { get; set; }
        public DateTime Time { get; set; }
        public int ParentMessageId { get; set; }
        public bool DisplaysToChannel { get; set; }
    }
}
