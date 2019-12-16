using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects.User
{
    public class User
    {
        public int Id { get; set; }
        public int      IconId { get; set; }
        public string?   Name { get; set; }
        public bool     IsOnline { get; set; }
        public string?   Password { get; set; }
        public string?   MailAddress { get; set; }
    }
}
