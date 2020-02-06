using System.Windows.Controls;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// ChannelListView.xaml の相互作用ロジック
	/// </summary>
	public partial class ChannelListView : UserControl {

		/// <summary>ViewModel.</summary>
		private ChannelListViewModel viewModel = new ChannelListViewModel();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ChannelListView() {
			InitializeComponent();
			this.DataContext = this.viewModel;
		}

	}
}

