using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    delegate void SelectedChannelHandler(int eventNum);
    static class MessageLogService
    {

        public static event SelectedChannelHandler CallMessageLog;
        public static void Fire(int eventNum)
        {
            CallMessageLog.Invoke(eventNum);
        }
    }
}
