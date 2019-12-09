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
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// ChannelListView.xaml の相互作用ロジック
	/// </summary>
	public partial class ChannelListView : UserControl {

		/// <summary>
		/// ViewModel.
		/// </summary>
		private ChannelListViewModel viewModel = new ChannelListViewModel();

		public ChannelListView() {
			InitializeComponent();

			this.DataContext = this.viewModel;
		}
	}
}
