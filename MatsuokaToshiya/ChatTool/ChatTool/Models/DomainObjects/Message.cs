using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects
{
    class Message
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public int UserId { get; set; }

        public string? Text { get; set; }

        public bool IsDisplay { get; set; }

        public DateTime Time { get; set; } = DateTime.Parse("1970 - 01 - 01 00:00:00");

        public Message() { }
        public Message(int id)
        {
            Id = id;
            UserName = "User" + Id.ToString();
            Text = "Test_Message";
        }
    }
}
