using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class ChannelsDisplayViewModel
    {
        public string LogInUserName { get; set; }

        public ChannelsDisplayViewModel()
        {
            LogInUserName = LogInService.LogInUser.Name;
        }

        public List<Button> GetChannelButtons()
        {
            var channelButtons = new List<Button>();

            var channels = ChannelDataProviderService.GetJoinedUser(LogInService.LogInUser.Id);

            foreach (Channel channel in channels)
            {
                var channelButton = new Button
                {
                    Content = channel.Name,
                    Name = "ChannelButton_" + channel.Id.ToString()
                };

                channelButton.Click += (s, e) =>
                {
                    ChannelDataProviderService.SelectingChannel = channel;
                };

                channelButtons.Add(channelButton);
            }

            return channelButtons;
        }
    }
}
