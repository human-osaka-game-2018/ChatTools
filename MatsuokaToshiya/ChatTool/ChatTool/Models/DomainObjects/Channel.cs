using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.DomainObjects
{
    class Channel
    {
        public int Id { get; set; } = 0;

        public string Name { get; set; } = "";

        public Channel(int id, string? name)
        {
            Id = id;
            Name = (name == null) ? "" : name;
        }
        public Channel() { }

    }
}
