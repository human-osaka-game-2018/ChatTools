using ChatTool.ViewModels;
using System.Windows.Controls;

namespace ChatTool.Views {
	/// <summary>
	/// InputAreaView.xaml の相互作用ロジック
	/// </summary>
	public partial class InputAreaView : UserControl {
		/// <summary>
		/// ViewModel.
		/// </summary>
		private InputAreaViewModel viewModel = new InputAreaViewModel();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public InputAreaView() {
			InitializeComponent();

			this.DataContext = this.viewModel;
		}

	}
}

