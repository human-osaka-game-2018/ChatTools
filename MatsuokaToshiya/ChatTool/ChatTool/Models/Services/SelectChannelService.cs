using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    static class SelectChannelService
    {
        public static Action<int> ChangeSelectChannel = (_) => { };
        public static int SelectingChannelId { get; set; }
        public static void CallMessageLogs(int channelId)
        {
            SelectingChannelId = channelId;
            ChangeSelectChannel.Invoke(channelId);
        }
    }
}
