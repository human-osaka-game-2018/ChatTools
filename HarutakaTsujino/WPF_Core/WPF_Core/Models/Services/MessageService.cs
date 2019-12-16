using System;
using System.Collections.Generic;
using System.Data;
using System.Reactive.Subjects;
using System.Text;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class MessageService
    { 
        public static List<Message> GetMessagesInChannel(int channelId)
        {
            using var messageTable = MessageDAO.Get(channelId);

            var messagesInChannel = new List<Message>();

            foreach (DataRow messageDataRow in messageTable.Rows)
            {
                var parentMessageId = messageDataRow["parent_message_id"];

                int? normalizedParentMessageId =
                    DBNull.Value.Equals(parentMessageId) ?
                    null :
                    (int?)parentMessageId;

                var channel = new Message(
                    (int)messageDataRow["id"],
                    (int)messageDataRow["channel_id"],
                    (int)messageDataRow["user_id"],
                    (string)messageDataRow["text"],
                    (DateTime)messageDataRow["time"],
                    normalizedParentMessageId);

                messagesInChannel.Add(channel);
            }

            return messagesInChannel;
        }
    }
}
