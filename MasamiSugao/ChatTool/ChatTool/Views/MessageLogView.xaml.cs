using ChatTool.ViewModels;
using System.Windows.Controls;

namespace ChatTool.Views {
	/// <summary>
	/// MessageLogView.xaml の相互作用ロジック
	/// </summary>
	public partial class MessageLogView : UserControl {

		/// <summary>
		/// ViewModel.
		/// </summary>
		private MessageLogViewModel viewModel = new MessageLogViewModel();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageLogView() {
			InitializeComponent();

			this.DataContext = this.viewModel;
			this.viewModel.ScrollToBottomAction = this.ScrollToBottom;
		}

		/// <summary>
		/// 1番下までスクロールする。
		/// </summary>
		private void ScrollToBottom() {
			this.messageListView.Dispatcher.Invoke(() => {
				if (this.messageListView.Items.Count > 0) {
					this.messageListView.ScrollIntoView(this.messageListView.Items[this.messageListView.Items.Count - 1]);
				}
			});
		}

	}
}

