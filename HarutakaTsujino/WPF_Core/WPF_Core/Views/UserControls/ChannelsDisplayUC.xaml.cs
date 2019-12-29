using System.Windows.Controls;
using WPF_Core.ViewModels.UserControls;

namespace WPF_Core.Views.UserControls
{
    /// <summary>
    /// ChannelsDisplayUC.xaml の相互作用ロジック
    /// </summary>
    public partial class ChannelsDisplayUC : UserControl
    {
        public ChannelsDisplayUC()
        {
            InitializeComponent();

            DataContext = channelsDisplayViewModel;
        }

        private void LBChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            channelsDisplayViewModel.ChangeSelectionChannel(sender, e);
        }

        private readonly ChannelsDisplayViewModel channelsDisplayViewModel = new ChannelsDisplayViewModel();
    }
}
