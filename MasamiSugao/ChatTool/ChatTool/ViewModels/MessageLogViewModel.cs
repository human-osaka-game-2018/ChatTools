using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
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
			// 複数スレッドからコレクション操作できるようにする
			BindingOperations.EnableCollectionSynchronization(this.Messages, new object());

			ChannelService.OnChannelChanged += (_, channel) => this.OnChannelChanged(channel);
			PeriodicMessageCheckService.OnNewMessagePosted += (_, __) => this.OnNewMessagePosted();

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
		/// チャンネル変更イベントハンドラ。
		/// </summary>
		private void OnChannelChanged(Channel channel) {
			PeriodicMessageCheckService.Enabled = false;
			var messages = this.service.ListMessagesBy(channel);

			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			this.Messages.Clear();
			messages.ForEach(x => this.Messages.Add(x));

			this.ScrollToBottomAction?.Invoke();

			PeriodicMessageCheckService.Enabled = true;
		}

		/// <summary>
		/// メッセージが増えた場合の処理。
		/// </summary>
		private void OnNewMessagePosted() {
			this.service.AddNewerMessagesTo(this.Messages, ChannelService.CurrentChannel!);
			this.ScrollToBottomAction?.Invoke();
		}
		#endregion

	}
}

