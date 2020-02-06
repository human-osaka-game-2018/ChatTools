using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {

	/// <summary>
	/// メッセージ画面のViewModel.
	/// </summary>
	public class MessageLogViewModel : BindableBase {

		#region field members
		/// <summary>メッセージログの種類。</summary>
		private MessageLogType messageLogType = MessageLogType.Main;

		/// <summary>サービスクラス。</summary>
		private MessageService service = new MessageService();

		/// <summary>新着メッセージ確認を定期実行するサービスクラス。</summary>
		private PeriodicMessageCheckService periodicMessageCheckService = new PeriodicMessageCheckService();

		/// <summary>選択中のメッセージ。</summary>
		private Message? selectedMessage;

		/// <summary>チャンネル表示チェックボックス表示有無。</summary>
		private bool isChkDisplaysToChannelVisible;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageLogViewModel() {
#if DEBUG
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				Enumerable.Range(1, 5).ToList().ForEach(i =>
					this.Messages.Add(new Message(i) { Text = "メッセージをここに表示する。", User = new User(0) { UserName = $"ユーザ名{i}" } })
				);
				return;
			}
#endif
			// 複数スレッドからの要素変更を許可する
			BindingOperations.EnableCollectionSynchronization(this.Messages, new object());
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

				switch (value) {
					case MessageLogType.Main:
						if (ChannelService.CurrentChannel != null) {
							this.ChangeTargetList(ChannelService.CurrentChannel);
						}
						ChannelService.OnChannelChanged += (_, channel) => this.ChangeTargetList(channel);
						MessageService.OnMessageThreadChanged += (_, message) => this.SelectedMessage = message;
						break;
					case MessageLogType.Thread:
						ChannelService.OnChannelChanged += (_, channel) => {
							this.periodicMessageCheckService.Enabled = false;
							this.Messages.Clear();
						};
						MessageService.OnMessageThreadChanged += (_, parentMessage) =>
							this.ChangeTargetList(ChannelService.CurrentChannel!, parentMessage);
						break;
				}
				this.periodicMessageCheckService.OnNewMessagePosted += (_, __) => this.OnNewMessagePosted();
			}
		}

		/// <summary>
		/// メッセージリスト。
		/// </summary>
		public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

		/// <summary>
		/// 選択中のメッセージ。
		/// </summary>
		public Message? SelectedMessage {
			get => this.selectedMessage;
			set {
				if (base.SetProperty(ref this.selectedMessage, value)) {
					if (this.MessageLogType == MessageLogType.Main) {
						MessageService.CurrentMessageThreadParent = value?.ParentMessage ?? value;
					}
				}
			}
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
		/// リスト表示対象を変更する。
		/// </summary>
		private void ChangeTargetList(Channel channel, Message? parentMessage = null) {
			this.periodicMessageCheckService.Enabled = false;

			if (this.MessageLogType == MessageLogType.Thread && parentMessage == null) return;

			var messages = this.MessageLogType switch {
				MessageLogType.Main => this.service.ListMessagesBy(channel),
				MessageLogType.Thread => this.service.ListMessagesBy(channel, parentMessage),
				_ => throw new NotImplementedException()
			};

			// 新着メッセージチェック処理に現在の最大IDを知らせる
			this.periodicMessageCheckService.ParentMessage = parentMessage;
			this.periodicMessageCheckService.MaxMessageId = (messages.Count == 0) ? -1 : messages.Max(x => x.Id!.Value);

			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			this.Messages.Clear();
			messages.ForEach(x => this.Messages.Add(x));

			this.periodicMessageCheckService.Enabled = true;
		}

		/// <summary>
		/// メッセージが増えた場合の処理。
		/// </summary>
		private void OnNewMessagePosted() {
			this.service.AddNewerMessagesTo(
				this.Messages,
				ChannelService.CurrentChannel!,
				(this.MessageLogType == MessageLogType.Thread) ? MessageService.CurrentMessageThreadParent : null
			);

			// 新着メッセージチェック処理に現在の最大IDを知らせる
			this.periodicMessageCheckService.MaxMessageId =
				(this.Messages.Count == 0) ? -1 : this.Messages.Max(x => x.Id!.Value);
		}
		#endregion

	}
}

