using ChatTool.Infrastructure.Database.ChannelDAO;
using ChatTool.Models.DomainObjects.Channel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.Services.Main
{
    public static class ChannelService
    {
        private static Channel currentChannel = new Channel();
        public static Channel CurrentChannel
        {
            get { return currentChannel; }
            set 
            {
                currentChannel = value;
                CurrentChannelChanged?.Invoke();
            }
        }

        public static List<Channel> GetChannels(int userId)
        {
            return ChannelDAO.GetLoginUserChannels(userId);
        }

        public static event Action? CurrentChannelChanged;               
    }
}
