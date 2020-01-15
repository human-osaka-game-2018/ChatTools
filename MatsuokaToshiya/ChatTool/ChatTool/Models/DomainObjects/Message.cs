using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace ChatTool.Models.DomainObjects
{
    class Message
    {
        public int Id { get; set; }

        public string UserName { get; set; } = "";

        public int UserId { get; set; }

        public string IconPath { get; set; } = "";

        public int ChannelId { get; set; }

        public string Text { get; set; } = "";

        public bool IsDisplay { get; set; }

        public DateTime Time { get; set; } = DateTime.Parse("1970 - 01 - 01 00:00:00");

        public Visibility ExistChild { get; set; } = Visibility.Hidden;

        public int ParentId { get; set; }

        private ObservableCollection<Message> child = new ObservableCollection<Message>();
        public ObservableCollection<Message> Child
        {
            get { return child; }
            set
            {
                if (value != this.child)
                    child = value;
            }
        }

        public Message() { }
    }
}
