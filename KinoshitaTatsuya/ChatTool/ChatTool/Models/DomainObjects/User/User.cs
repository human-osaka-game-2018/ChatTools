using ChatTool.Models.Services.Main;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects.User
{
    public class User
    {
        public int       Id { get; set; }

        private int iconId;
        public int  IconId 
        {
            get { return iconId; }
            set 
            {
                iconId = value;
                IconPath = IconPathFactory.Create(iconId);
            } 
        }

        public string?   Name { get; set; }
        public bool      IsOnline { get; set; }
        public string?   Password { get; set; }
        public string?   MailAddress { get; set; }
        public string?   IconPath { get; set; }
    }
}
