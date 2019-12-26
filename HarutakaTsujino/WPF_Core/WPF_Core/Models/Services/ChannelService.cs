using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class ChannelService
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

        public static List<Channel> GetChannelsJoinedUser(User user)
        {
            using var channelMemberTable = ChannelMemberDAO.GetIdsJoinedUser(user.Id);

            var ids = channelMemberTable.AsEnumerable()
                .Select(x => x.Field<int>("channel_id"));

            using var channelTable = ChannelDAO.Get(ids.ToList());

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
