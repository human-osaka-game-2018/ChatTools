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
        public string ParticipatedUser(ObservableCollection<Channel> Channels)
        {
            var channelDao = new ChannelDAO();
            if (null == LoginService.User || null == LoginService.User.Name) return "";
            channelDao.ParticipatedUser(Channels, LoginService.User.Id);
            return LoginService.User.Name;

        }
    }
}
