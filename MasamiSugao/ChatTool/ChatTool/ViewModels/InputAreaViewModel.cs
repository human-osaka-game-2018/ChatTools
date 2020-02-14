using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// 入力画面のViewModel.
	/// </summary>
	public class InputAreaViewModel : BindableBase {

		#region constants/readonly
		/// <summary>サービスクラス。</summary>
		private MessageService service = new MessageService();
		#endregion

		#region field members
		/// <summary>メッセージログの種類。</summary>
		private MessageLogType messageLogType = MessageLogType.Main;

		/// <summary>入力文字列。</summary>
		private string inputMessage = string.Empty;

		/// <summary>チャンネルにも表示するかどうか。(スレッド画面用)</summary>
		private bool displaysToChannel;

		/// <summary>チャンネル表示チェックボックス表示有無。</summary>
		private bool isChkDisplaysToChannelVisible = true;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public InputAreaViewModel() {
			this.BtnSendClickCommand = new DelegateCommand(() => this.OnBtnSendClicked(), () => this.CanSend);

			ChannelService.OnChannelChanged += (_, __) => this.InitializeInputItems();
			MessageService.OnMessagePosted += (_, message) => this.OnMessagePosted(message);
			MessageService.OnMessageThreadChanged += (_, __) => this.OnMessageThreadChanged();
		}
		#endregion

		#region properties
		/// <summary>
		/// メッセージログの種類。
		/// </summary>
		public MessageLogType MessageLogType {
			get => this.messageLogType;
			set {
				this.messageLogType = value;
				this.IsChkDisplaysToChannelVisible = (value == MessageLogType.Thread);
			}
		}

		/// <summary>
		/// 入力文字列。
		/// </summary>
		public string InputMessage {
			get => this.inputMessage;
			set {
				if (base.SetProperty(ref this.inputMessage, value)) {
					this.BtnSendClickCommand.RaiseCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// 送信ボタン押下コマンド。
		/// </summary>
		public DelegateCommand BtnSendClickCommand { get; }

		/// <summary>
		/// 送信可能かどうかを示すフラグ。
		/// </summary>
		public bool CanSend => ChannelService.CurrentChannel == null || this.inputMessage.Length > 0;

		/// <summary>
		/// チャンネルにも表示するかどうか。(スレッド画面用)
		/// </summary>
		public bool DisplaysToChannel {
			get => this.displaysToChannel;
			set => base.SetProperty(ref this.displaysToChannel, value);
		}

		/// <summary>
		/// チャンネル表示チェックボックス表示有無。
		/// </summary>
		public bool IsChkDisplaysToChannelVisible {
			get => this.isChkDisplaysToChannelVisible;
			set => base.SetProperty(ref this.isChkDisplaysToChannelVisible, value);
		}
		#endregion

		#region private methods
		/// <summary>
		/// 送信ボタン押下時の処理。
		/// </summary>
		private void OnBtnSendClicked() {
			var message = new Message(
				ChannelService.CurrentChannel!,
				LoginService.CurrentUser!,
				this.InputMessage,
				(this.MessageLogType == MessageLogType.Main) ? true : this.DisplaysToChannel,
				(this.MessageLogType == MessageLogType.Main) ? null : MessageService.ParentOfCurrentMessageThread
			);

			this.service.Post(message);
		}

		/// <summary>
		/// メッセージ送信完了時の処理。
		/// </summary>
		/// <param name="message">送信メッセージ</param>
		private void OnMessagePosted(Message message) {
			// 対応するViewと、投稿されたメッセージが同じ画面（メインorスレッド）の場合は入力欄初期化
			if ((this.messageLogType == MessageLogType.Main) == (message.ParentMessage == null)) {
				this.InitializeInputItems();
			}
		}

		/// <summary>
		/// メッセージスレッド変更時の処理。
		/// </summary>
		private void OnMessageThreadChanged() {
			if (this.MessageLogType == MessageLogType.Main) return;

			this.InitializeInputItems();
		}

		/// <summary>
		/// 入力項目の初期化を行う。
		/// </summary>
		private void InitializeInputItems() {
			this.InputMessage = string.Empty;
			this.DisplaysToChannel = false;
		}
		#endregion

	}
}

