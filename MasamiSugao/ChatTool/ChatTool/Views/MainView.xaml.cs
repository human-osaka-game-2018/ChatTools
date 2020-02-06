using System.Windows;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// MainView.xaml の相互作用ロジック
	/// </summary>
	public partial class MainView : Window {

		/// <summary>ViewModl.</summary>
		private MainViewModel viewModel = new MainViewModel();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MainView() {
			InitializeComponent();
			this.DataContext = this.viewModel;

			this.mainMessagePaneView.MessageLogType = MessageLogType.Main;
			this.threadPainMessageRWView.MessageLogType = MessageLogType.Thread;
		}

	}
}

