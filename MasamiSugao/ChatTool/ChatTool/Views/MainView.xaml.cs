using System.Windows;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// MainView.xaml の相互作用ロジック。
	/// </summary>
	public partial class MainView : Window {

		/// <summary>ViewModel.</summary>
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

		/// <summary>
		/// 画面を閉じるイベントハンドラ。
		/// </summary>
		/// <param name="sender">イベントソース</param>
		/// <param name="e">イベントパラメータ</param>
		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			this.viewModel.OnClosing();
		}

	}
}

