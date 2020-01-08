using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive.Subjects;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services.Factories;

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
                if (selectingChannel?.Id == value?.Id) return;

                onSelectingChangedAsSubject?.OnNext(selectingChannel = value);
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
                .Select(x => ChannelFactory.Create(x));

            return channelsJoinedUser;
        }

        private static Channel? selectingChannel;

        private static readonly BehaviorSubject<Channel?> onSelectingChangedAsSubject = new BehaviorSubject<Channel?>(null);
    }
}
