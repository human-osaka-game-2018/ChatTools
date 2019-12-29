using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using WPF_Core.Models.DomainObjects;
using WPF_Core.Models.Services;

namespace WPF_Core.ViewModels.UserControls
{
    public class ChannelsDisplayViewModel
    {
        public string LogInUserName { get; set; }

        public ObservableCollection<Channel> Channels { get; } 
            = new ObservableCollection<Channel>();

        public ChannelsDisplayViewModel()
        {
            LogInUserName = LogInService.LogInUser!.Name;

            var channels = ChannelService.GetChannelsJoinedUser(LogInService.LogInUser);

            Channels = new ObservableCollection<Channel>(channels);

            var defaultChannel = channels.FirstOrDefault();

            if (defaultChannel is null) return;

            ChannelService.SelectingChannel = defaultChannel;
        }

        public void ChangeSelectionChannel(object sender, SelectionChangedEventArgs e)
        {
            ChannelService.SelectingChannel = e.AddedItems[0] as Channel;
        }
    }
}
