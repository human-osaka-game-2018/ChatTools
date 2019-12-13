using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects
{
    class ChannelMember
    {
        public int Id { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
