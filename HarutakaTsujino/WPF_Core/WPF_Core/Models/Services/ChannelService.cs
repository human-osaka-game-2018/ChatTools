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
        public static IObservable<Channel?> OnSelectingChangedAsObservable => onSelectingChangedAsSubject;

        public static Channel? SelectingChannel
        {
            get => selectingChannel;
            set
            {
                if (selectingChannel?.Id != value?.Id)
                {
                    selectingChannel = value;

                    if (selectingChannel is null) return;

                    onSelectingChangedAsSubject?.OnNext(selectingChannel);
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

            return channelsJoinedUser;
        }

        private static Channel? selectingChannel;

        private static readonly BehaviorSubject<Channel?> onSelectingChangedAsSubject = new BehaviorSubject<Channel?>(null);
    }
}
