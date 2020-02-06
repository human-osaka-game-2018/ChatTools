using System.ComponentModel;
using System.Windows;
using ChatTool.Models;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {

	#region enums
	/// <summary>
	/// メッセージログの種類。
	/// </summary>
	public enum MessageLogType {
		/// <summary>メイン。</summary>
		Main,
		/// <summary>スレッド。</summary>
		Thread
	}
	#endregion

	/// <summary>
	/// Main画面のViewModel。
	/// </summary>
	public class MainViewModel : BindableBase {

		#region constants
		/// <summary>スレッドペインの最大表示幅。</summary>
		private const int ThreadPaneMaxWidth = 300;
		#endregion

		#region field members
		/// <summary>スレッドペインの表示幅。</summary>
		private int threadPaneWidth;

		/// <summary>スレッドペインを表示するかどうか。</summary>
		private bool isThreadPaneVisible;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MainViewModel() {
			this.Title = App.AppName;
			this.BtnCloseThreadPainClickCommand = new DelegateCommand(() => this.IsThreadPaneVisible = false);
			ChannelService.OnChannelChanged += (_, __) => this.IsThreadPaneVisible = false;
			MessageService.OnMessageThreadChanged += (_, message) => this.IsThreadPaneVisible = (message != null);

#if DEBUG
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				this.IsThreadPaneVisible = true;
				return;
			}
#endif
		}
		#endregion

		#region properties
		/// <summary>
		/// ウィンドウタイトル。
		/// </summary>
		public string Title { get; }

		/// <summary>
		/// スレッドペインの表示幅。
		/// </summary>
		public int ThreadPaneWidth {
			get => this.threadPaneWidth;
			set => base.SetProperty(ref this.threadPaneWidth, value);
		}

		/// <summary>
		/// スレッドペインを閉じるボタンクリックコマンド。
		/// </summary>
		public DelegateCommand BtnCloseThreadPainClickCommand { get; }

		/// <summary>
		/// スレッドペインを表示するかどうか。
		/// </summary>
		private bool IsThreadPaneVisible {
			get => this.isThreadPaneVisible;
			set {
				if (this.isThreadPaneVisible == value) return;

				this.isThreadPaneVisible = value;
				this.ThreadPaneWidth = value ? ThreadPaneMaxWidth : 0;

				if (!value) {
					MessageService.CurrentMessageThreadParent = null;
				}
			}
		}
		#endregion

	}
}

