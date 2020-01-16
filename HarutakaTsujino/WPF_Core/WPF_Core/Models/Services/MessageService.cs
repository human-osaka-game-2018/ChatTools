using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services.Factories;

namespace WPF_Core.Models.Services
{
    static class MessageService
    {
        public static IObservable<IEnumerable<Message?>> OnMessagesFetchedAsObservable => onMessagesFetchedAsSubject;

        public static IObservable<Unit> OnMessagePostedAsObservable => onMessagePostedAsSubject;

        static MessageService()
        {
            ChannelService.OnSelectingChangedAsObservable
                .Subscribe((channel) =>
                {
                    if (channel is null) return;

                    onMessagesFetchedAsSubject.OnNext(Get(channel));
                });

            _ = FetchMessagesContinuousAsync();
        }

        public static IEnumerable<Message?> Get(Channel channel)
        {
            using var messageTable = MessageDAO.GetWithChannelId(channel.Id);

            var messages = messageTable.AsEnumerable()
                .Select(x => MessageFactory.Create(x));

            return messages;
        }

        public static void Post(string text, User user, Channel channel, Message? parentMessage = null)
        {
            MessageDAO.Post(text, user.Id, channel.Id, parentMessage?.Id);

            onMessagesFetchedAsSubject.OnNext(Get(channel));

            onMessagePostedAsSubject.OnNext(Unit.Default);
        }

        private static async Task FetchMessagesContinuousAsync()
        {
            while (true)
            {
                // UIスレッドで扱う必要のあるメソッドがあるためConfigureAwaitは使わない
                await Task.Run(() => System.Threading.Thread.Sleep((int)(MESSAGE_UPDATE_TIME_SPAN_S * 1000)));

                var messagesTask = await Task.Run(() =>
                {
                    var currentChannel = ChannelService.SelectingChannel;

                    return currentChannel is null ? new List<Message?>() : Get(currentChannel);
                });

                onMessagesFetchedAsSubject.OnNext(messagesTask);
            }
        }

        private const double MESSAGE_UPDATE_TIME_SPAN_S = 1.0;

        private static readonly BehaviorSubject<IEnumerable<Message?>> onMessagesFetchedAsSubject =
            new BehaviorSubject<IEnumerable<Message?>>(new List<Message?>());

        private static readonly Subject<Unit> onMessagePostedAsSubject = new Subject<Unit>();
    }
}
