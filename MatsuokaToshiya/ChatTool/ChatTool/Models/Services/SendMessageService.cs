﻿using ChatTool.Infrastructure.Database;
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
            MessageDAO.SendMessage(inputText, channelId);
            ReadMessageService.RefleshMessageLog?.Invoke();
        }

        public void SendReplyMessage(string inputText,int parentId)
        {
            var channelId = SelectChannelService.SelectingChannelId;
            MessageDAO.SendReplyMessage(inputText, channelId,parentId);
            ReadMessageService.RefleshMessageLog?.Invoke();
        }

    }
}
