﻿using System.Windows;
using System.Windows.Controls;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// InputAreaView.xaml の相互作用ロジック。
	/// </summary>
	public partial class InputAreaView : UserControl {

		#region constants/readonly
		/// <summary>
		/// MessageLogType依存プロパティ。
		/// </summary>
		public static readonly DependencyProperty MessageLogTypeProperty = DependencyProperty.Register(
																		"MessageLogType",
																		typeof(MessageLogType),
																		typeof(InputAreaView),
																		new FrameworkPropertyMetadata(OnMessageLogTypePropertyChanged));
		#endregion

		#region field members
		/// <summary>ViewModel.</summary>
		private InputAreaViewModel viewModel = new InputAreaViewModel();
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public InputAreaView() {
			InitializeComponent();
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
			var view = d as InputAreaView;
			view!.viewModel.MessageLogType = (MessageLogType)e.NewValue;
		}
		#endregion

	}
}

