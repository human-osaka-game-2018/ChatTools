using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using WPF_Core.Infrastructure.Database;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services.Factories;

namespace WPF_Core.Models.Services
{
    static class MessageService
    {
        public static IObservable<Unit> OnMessagePostedAsObservable => onMessagePostedAsSubject;

        public static IEnumerable<Message?> Get(Channel channel)
        {
            using var messageTable = MessageDAO.GetWithChannelId(channel.Id);

            var messages = messageTable.AsEnumerable()
                .Select(x =>
                {            
                    return MessageFactory.Create(x);
                });

            return messages;
        }

        public static void Post(string text, User user, Channel channel, Message? parentMessage = null)
        {
            MessageDAO.Post(text, user.Id, channel.Id, parentMessage?.Id);

            onMessagePostedAsSubject.OnNext(Unit.Default);
        }

        private static readonly Subject<Unit> onMessagePostedAsSubject = new Subject<Unit>();
    }
}
