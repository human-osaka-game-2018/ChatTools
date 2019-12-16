using System;
using System.Collections.Generic;
using System.Text;

namespace WPF_Core.Models.DomainObjects
{
    public class Channel
    {
        public Channel(int id, string name)
        {
            Id = id;
            
            Name = name;
        }

        public int Id { get; private set; }
        
        public string Name { get; private set; }
    }
}
