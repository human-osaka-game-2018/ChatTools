using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ChatTool.Models.Services
{
    delegate void RefleshMessageLogEvent();
    static class CallMessageService
    {
        public static RefleshMessageLogEvent RefleshMessageLog = ()=> { };
        public static void CallMessages(ObservableCollection<Message> messages,int channelId)
        {
            messages.Clear();
            var messageDao = new MessageDAO();
            messageDao.MessageList(messages, channelId);
        }

    }
}
