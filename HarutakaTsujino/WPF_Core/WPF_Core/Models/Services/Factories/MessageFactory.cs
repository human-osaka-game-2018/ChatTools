using System;
using System.Data;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services.Factories
{
    static class MessageFactory
    {
        public static Message? Create(DataRow dataRow)
        {
            var channelDataRows = ChannelDAO.Get(dataRow.Field<int>("channel_id")).Rows;
            if (channelDataRows.Count is 0) return null;

            var userDataRows = UserDAO.GetWithId(dataRow.Field<int>("user_id")).Rows;
            if (userDataRows.Count is 0) return null;

            var parentMessageId = dataRow.Field<object>("parent_message_id");

            Message? parentMessage = null;

            if (parentMessageId != null)
            {
                var MessageDataRows = MessageDAO.GetWithId((int)parentMessageId).Rows;
                if (MessageDataRows.Count is 0) return null;

                parentMessage = Create(MessageDataRows[0]);
            }

            return new Message(
                dataRow.Field<int>("id"),
                ChannelFactory.Create(channelDataRows[0]),
                UserFactory.Create(userDataRows[0]),
                dataRow.Field<string>("text"),
                dataRow.Field<DateTime>("time"),
                parentMessage);
        }
    }
}
