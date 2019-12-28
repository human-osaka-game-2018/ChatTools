using ChatTool.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatTool.Models.Services
{
    class SendMessageService
    {
        public void SendMessage(string inputText)
        {
            var channelId = SelectChannelService.SelectingChannelId;
            var messageDao = new MessageDAO();
            messageDao.SendMessage(inputText, channelId);
            CallMessageService.RefleshMessageLog?.Invoke();
        }
        public void SendReplyMessage(string inputText,int parentId)
        {
            var channelId = SelectChannelService.SelectingChannelId;
            var messageDao = new MessageDAO();
            messageDao.SendReplyMessage(inputText, channelId,parentId);
            CallMessageService.RefleshMessageLog?.Invoke();
        }

    }
}
