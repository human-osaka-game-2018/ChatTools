using System.Windows;
using System.Windows.Controls;
using ChatTool.Models.DomainObjects;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// MessagePanelView.xaml の相互作用ロジック。
	/// </summary>
	public partial class MessagePanelView : UserControl {

		#region constants/readonly
		/// <summary>Message依存プロパティ。</summary>
		public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
																		"Message",
																		typeof(Message),
																		typeof(MessagePanelView),
																		new FrameworkPropertyMetadata(null, OnMessagePropertyChanged));
		#endregion

		#region field members
		/// <summary>ViewModel.</summary>
		private MessagePanelViewModel viewModel = new MessagePanelViewModel();
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessagePanelView() {
			InitializeComponent();

			this.DataContext = this.viewModel;
		}
		#endregion

		#region properties
		/// <summary>
		/// 表示するメッセージ。
		/// </summary>
		public Message Message {
			get => (Message)base.GetValue(MessageProperty);
			set => base.SetValue(MessageProperty, value);
		}
		#endregion

		#region private static methods
		/// <summary>
		/// <see cref="Message"/> 依存プロパティ変更イベントハンドラ。
		/// </summary>
		/// <param name="d">値が変更されたプロパティを持つインスタンス</param>
		/// <param name="e">イベントパラメータ</param>
		private static void OnMessagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var message = e.NewValue as Message;
			var view = d as MessagePanelView;
			view!.viewModel.Message = message;
		}
		#endregion

	}
}

