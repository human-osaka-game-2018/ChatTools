using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_Core.Common;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class ChannelsDisplayViewModel
    {
        public string LogInUserName { get; set; }

        public ObservableCollection<Channel> Channels { get; }

        public ChannelsDisplayViewModel()
        {
            LogInUserName = LogInService.LogInUser.Name;

            var channels = ChannelService.GetJoinedUser(LogInService.LogInUser.Id);

            Channels = new ObservableCollection<Channel>(channels);
        }

        public void ChangeSelectionChannel(object sender, SelectionChangedEventArgs e)
        {
            ChannelService.SelectingChannel = e.AddedItems[0] as Channel;
        }
    }
}
