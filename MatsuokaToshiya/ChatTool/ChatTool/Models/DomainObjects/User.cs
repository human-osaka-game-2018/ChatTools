using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects
{
    class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? MailAddress { get; set; }

        public string? Password { get; set; }

        public int IconId { get; set; }

        public string IconPath { get; set; } = "";

        public bool IsOnline { get; set; }

    }
}
