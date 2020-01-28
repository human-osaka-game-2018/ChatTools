using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    class ChannelService
    {
        public void GetAvailableByUserid(ObservableCollection<Channel> Channels,int userId)
        {
            ChannelDAO.GetAvailableByUserid(Channels, userId);

        }
    }
}
