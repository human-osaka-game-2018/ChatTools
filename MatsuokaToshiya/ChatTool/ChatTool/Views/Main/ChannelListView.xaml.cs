﻿using ChatTool.ViewModels.Main;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            var viewModel = new ChannelViewModel();
            this.DataContext = viewModel;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           ((ChannelViewModel)DataContext).ChannelSelected();
        }
    }
}