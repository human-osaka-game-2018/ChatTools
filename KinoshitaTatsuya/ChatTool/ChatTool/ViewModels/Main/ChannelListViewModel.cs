using System;
using System.Collections.Generic;
using System.Text;
using ChatTool.Infrastructure.Database.ChannelDAO;
using ChatTool.Models.DomainObjects.Channel;
using ChatTool.Models.Services.LoginService;
using ChatTool.Models.Services.Main;

namespace ChatTool.ViewModels.Main
{
    public class ChannelListViewModel
    {
        public List<Channel> Channels { get; set; } = new List<Channel>();

        public ChannelListViewModel()
        {            
            Channels = ChannelDAO.GetLoginUserChannels(LoginService.LoginUser.Id);            
        }
        
        public void UpdateCurrentChannel(int selectItemIndex)
        {
            ChannelService.CurrentChannel = Channels[selectItemIndex];
        }
    }
}
