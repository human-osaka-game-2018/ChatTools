﻿using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// MessageLogView.xaml の相互作用ロジック。
	/// </summary>
	public partial class MessageLogView : UserControl {

		#region constants/readonly
		/// <summary>
		/// MessageLogType依存プロパティ。
		/// </summary>
		public static readonly DependencyProperty MessageLogTypeProperty = DependencyProperty.Register(
																			"MessageLogType",
																			typeof(MessageLogType),
																			typeof(MessageLogView),
																			new FrameworkPropertyMetadata(OnMessageLogTypePropertyChanged));
		#endregion

		#region field members
		/// <summary>ViewModel.</summary>
		private MessageLogViewModel viewModel = new MessageLogViewModel();
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageLogView() {
			InitializeComponent();
			((INotifyCollectionChanged)this.messageListView.Items).CollectionChanged += this.OnCollectionOfMessageListViewChanged;

			this.DataContext = this.viewModel;
			this.viewModel.MessageLogType = this.MessageLogType;
		}
		#endregion

		#region properties
		/// <summary>
		/// メッセージログの種類。
		/// </summary>
		public MessageLogType MessageLogType {
			get => (MessageLogType)base.GetValue(MessageLogTypeProperty);
			set => base.SetValue(MessageLogTypeProperty, value);
		}
		#endregion

		#region private static methods
		/// <summary>
		/// MessageLogType依存プロパティ変更イベントハンドラ。
		/// </summary>
		/// <param name="d">イベントソース</param>
		/// <param name="e">イベントパラメータ</param>
		private static void OnMessageLogTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			var view = d as MessageLogView;
			view!.viewModel.MessageLogType = (MessageLogType)e.NewValue;
		}
		#endregion

		#region private methods
		/// <summary>
		/// <see cref="messageListView"/> のリスト変更イベント。
		/// </summary>
		private void OnCollectionOfMessageListViewChanged(object sender, NotifyCollectionChangedEventArgs e) {
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
				case NotifyCollectionChangedAction.Reset:
					Application.Current.Dispatcher.Invoke(() => {
						// 1番下までスクロールする。
						if (this.messageListView.HasItems) {
							this.messageListView.ScrollIntoView(this.messageListView.Items[this.messageListView.Items.Count - 1]);
						}
					});
					break;
			}
		}
		#endregion

	}
}

