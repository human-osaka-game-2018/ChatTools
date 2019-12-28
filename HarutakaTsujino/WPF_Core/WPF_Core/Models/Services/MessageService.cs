﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class MessageService
    { 
        public static IEnumerable<Message> GetMessagesInChannel(Channel channel)
        {
            using var messageTable = MessageDAO.Get(channel.Id);

            var messagesInChannel = new List<Message>();

            var messages = messageTable.AsEnumerable()
                .Select(x =>
                {
                    var parentMessageId = x.Field<object>("parent_message_id");

                    return new Message(
                        x.Field<int>("id"),
                        x.Field<int>("channel_id"),
                        x.Field<int>("user_id"),
                        x.Field<string>("text"),
                        x.Field<DateTime>("time"),
                        DBNull.Value.Equals(parentMessageId) ? null : (int?)parentMessageId);
                });

            return messages;
        }
    }
}
