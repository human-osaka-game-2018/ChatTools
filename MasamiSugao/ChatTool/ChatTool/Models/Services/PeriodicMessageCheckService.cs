using System;
using System.Threading.Tasks;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Models.Services {
	/// <summary>
	/// 新着メッセージ確認を定期実行するサービスクラス。
	/// </summary>
	public class PeriodicMessageCheckService {

		#region constants
		/// <summary>メッセージチェックを行う間隔(秒)</summary>
		private const int MessageCheckIntervalSeconds = 5;
		#endregion

		#region field members
		/// <summary>定期実行処理を稼働させるかどうか。</summary>
		private bool enabled;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public PeriodicMessageCheckService() {
			MessageService.OnMessagePosted += (_, __) => ForceToCheck();
		}
		#endregion

		#region events
		/// <summary>
		/// 新着メッセージ発見イベント。
		/// </summary>
		public event EventHandler? OnNewMessagePosted;
		#endregion

		#region properties
		/// <summary>
		/// 定期実行処理を稼働させるかどうか。
		/// </summary>
		public bool Enabled {
			get => this.enabled;
			set {
				if (this.enabled == value) return;

				this.enabled = value;
				if (value) {
					this.CheckPeriodicallyAsync();
				}
			}
		}

		/// <summary>
		/// スレッドを対象とする場合の親メッセージ。
		/// </summary>
		public Message? ParentMessage { get; set; } = null;

		/// <summary>
		/// 取得完了しているMessageIDの最大値。
		/// </summary>
		public int MaxMessageId { get; set; } = -1;
		#endregion

		#region private methods
		/// <summary>
		/// 強制的に新着メッセージ確認処理を実行する。
		/// </summary>
		private void ForceToCheck() {
			this.CheckNewMessages();
		}

		/// <summary>
		/// 新着メッセージ確認処理。
		/// </summary>
		private void CheckNewMessages() {
			var dao = new MessageDAO();
			if (ChannelService.CurrentChannel != null) {
				var id = this.ParentMessage switch {
					null => dao.SelectMaxMessageId(ChannelService.CurrentChannel),
					_ => dao.SelectMaxMessageId(this.ParentMessage)
				};

				if (this.MaxMessageId < id) {
					this.MaxMessageId = id;
					this.OnNewMessagePosted?.Invoke(null, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// 定期的に新着メッセージ確認処理を実行する。
		/// </summary>
		private async void CheckPeriodicallyAsync() {
			while (Enabled) {
				CheckNewMessages();
				await Task.Delay(MessageCheckIntervalSeconds).ConfigureAwait(false);
			}
		}
		#endregion

	}
}

