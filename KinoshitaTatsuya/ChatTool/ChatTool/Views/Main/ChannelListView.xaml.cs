using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatTool.ViewModels.Main;

namespace ChatTool.Views.Main
{
    /// <summary>
    /// ChannelListView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChannelListView : UserControl
    {
        public ChannelListView()
        {
            InitializeComponent();
            DataContext = channelListViewModel;
        }

        private void OnSelected(object sender, SelectionChangedEventArgs e)
        {
            channelListViewModel.UpdateCurrentChannel(ListChannelName.SelectedIndex);
        }

        private ChannelListViewModel channelListViewModel = new ChannelListViewModel();
    }
}
