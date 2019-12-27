using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    delegate void SelectedChannelHandler(int value);
    static class SelectChannelService
    {
        public static int SelectingChannelId { get; set; }
        public static event SelectedChannelHandler CallMessageLog = (_) => { };
        public static void CallMessageLogs(int channelId)
        {
            SelectingChannelId = channelId;
            CallMessageLog?.Invoke(channelId);
        }
    }
}
