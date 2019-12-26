using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;

namespace WPF_Core.Models.Services
{
    static class ChannelService
    {
        public static IObservable<int> OnSelectingChangedAsObservable => onSelectingChangedAsSubject;

        public static Channel? SelectingChannel
        {
            get
            {
                return selectingChannel;
            }

            set
            {
                if (value is null) return;

                if (selectingChannel is null || selectingChannel.Id != value.Id)
                {
                    selectingChannel = value;

                    onSelectingChangedAsSubject?.OnNext(selectingChannel.Id);
                }
            }
        }

        public static IEnumerable<Channel>? GetChannelsJoinedUser(User user)
        {
            using var channelMemberTable = ChannelMemberDAO.GetIdsJoinedUser(user.Id);

            var ids = channelMemberTable.AsEnumerable()
                .Select(x => x.Field<int>("channel_id"));

            using var channelTable = ChannelDAO.Get(ids);

            if (channelTable is null) return null;

            var channelsJoinedUser = channelTable.AsEnumerable()
                .Select(x =>
                {
                    return new Channel(
                       x.Field<int>("id"),
                       x.Field<string>("channel_name"));
                });

            if (channelsJoinedUser.Count() <= 0) return null;

            return channelsJoinedUser;
        }

        private static Channel? selectingChannel;

        private static readonly Subject<int> onSelectingChangedAsSubject = new Subject<int>();
    }
}
