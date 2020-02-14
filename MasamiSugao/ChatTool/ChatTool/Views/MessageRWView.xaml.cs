using System.Windows;
using System.Windows.Controls;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// MessageRWView.xaml の相互作用ロジック。
	/// </summary>
	public partial class MessageRWView : UserControl {

		/// <summary>
		/// MessageLogType依存プロパティ。
		/// </summary>
		public static readonly DependencyProperty MessageLogTypeProperty = DependencyProperty.Register(
																				"MessageLogType",
																				typeof(MessageLogType),
																				typeof(MessageRWView),
																				new FrameworkPropertyMetadata(MessageLogType.Main));

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageRWView() {
			InitializeComponent();
		}

		/// <summary>
		/// メッセージログの種類。
		/// </summary>
		public MessageLogType MessageLogType {
			get => (MessageLogType)base.GetValue(MessageLogTypeProperty);
			set => base.SetValue(MessageLogTypeProperty, value);
		}

	}
}

