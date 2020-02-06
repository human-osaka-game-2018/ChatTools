using System.Windows;
using System.Windows.Controls;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Views {
	/// <summary>
	/// MessagePanelView.xaml の相互作用ロジック
	/// </summary>
	public partial class MessagePanelView : UserControl {

		/// <summary>
		/// Message依存プロパティ。
		/// </summary>
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
																		"Message",
																		typeof(Message),
																		typeof(MessagePanelView),
																		new FrameworkPropertyMetadata(null));

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessagePanelView() {
			InitializeComponent();
		}

		/// <summary>
		/// 表示するメッセージ。
		/// </summary>
		public Message Message {
			get => (Message)base.GetValue(MessageProperty);
			set => base.SetValue(MessageProperty, value);
		}

	}
}

