using System;
using System.Collections.Generic;
using System.Data;
using System.Reactive.Subjects;
using System.Text;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class ChannelDataProviderService
    {
        public static IObservable<int> OnSelectingChangedAsObservable => onSelectingChangedAsSubject;

        public static Channel SelectingChannel
        {
            get
            {
                return selectingChannel;
            }

            set
            {
                if (selectingChannel == null || selectingChannel.Id != value.Id)
                {
                    selectingChannel = value;

                    onSelectingChangedAsSubject?.OnNext(selectingChannel.Id);
                }
            }
        }

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

        public static List<Channel> GetJoinedUser(int id)
        {
            using var channelTable = ChannelDAO.GetJoinedUser(id);

            var channelsJoinedUser = new List<Channel>();

            foreach (DataRow channelDataRow in channelTable.Rows)
            {
                var channel = new Channel(
                    (int)channelDataRow["id"],
                    (string)channelDataRow["channel_name"]);

                channelsJoinedUser.Add(channel);
            }

            return channelsJoinedUser;
        }

        private static Channel selectingChannel;

        private static readonly Subject<int> onSelectingChangedAsSubject = new Subject<int>();
    }
}
