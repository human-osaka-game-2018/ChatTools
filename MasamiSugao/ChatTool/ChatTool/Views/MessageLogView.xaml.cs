using System.Windows.Controls;
using ChatTool.ViewModels;

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

			this.DataContext = viewModel;
		}

	}
}
