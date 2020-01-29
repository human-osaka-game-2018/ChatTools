using System;
using System.Collections.ObjectModel;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// メッセージ画面のViewModel.
	/// </summary>
	public class MessageLogViewModel : BindableBase {

		#region field members
		/// <summary>サービスクラス。</summary>
		private MessageService service = new MessageService();

		/// <summary>選択中のメッセージ。</summary>
		private Message? selectedMessage;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageLogViewModel() {
			ChannelService.OnChannelChanged += (_, channel) => this.OnChannelChanged(channel);
			MessageService.OnMessagePosted += (_, __) => this.OnMessagePosted();

			this.Messages.Add(new Message(0) { Text = "チャンネルを選択してください。", User = new User(0) { UserName = "ChatTool Bot" } });
		}
		#endregion

		#region properties
		public Action? ScrollToBottomAction { get; set; }

		/// <summary>
		/// メッセージリスト。
		/// </summary>
		public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

		public Message? SelectedMessage {
			get => this.selectedMessage;
			set {
				base.SetProperty(ref this.selectedMessage, value);
			}
		}
		/// <summary>
		/// 画面ロードコマンド。
		/// </summary>
		//public DelegateCommand LoadedCommand { get; }
		#endregion

		#region private methods
		/// <summary>
		/// 画面ロードイベントハンドラ。
		/// </summary>
		private void OnChannelChanged(Channel channel) {
			var messages = this.service.ListMessagesBy(channel);
			this.Messages.Clear();
			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			messages.ForEach(x => this.Messages.Add(x));
			this.ScrollToBottomAction?.Invoke();
		}

		/// <summary>
		/// メッセージ投稿時の処理。
		/// </summary>
		private void OnMessagePosted() {
			this.service.AddNewerMessagesTo(this.Messages, ChannelService.CurrentChannel!);
			this.ScrollToBottomAction?.Invoke();
		}
		#endregion

	}
}
