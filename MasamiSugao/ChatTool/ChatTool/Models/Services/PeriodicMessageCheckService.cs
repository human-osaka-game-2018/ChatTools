using System;
using System.Threading.Tasks;
using ChatTool.Infrastructure.Database;

namespace ChatTool.Models.Services {
	/// <summary>
	/// 新着メッセージ確認を定期実行するサービスクラス。
	/// </summary>
	public static class PeriodicMessageCheckService {

		#region constants
		/// <summary>メッセージチェックを行う間隔(秒)</summary>
		private const int MessageCheckIntervalSeconds = 5;
		#endregion

		#region field members
		/// <summary>定期実行処理を稼働させるかどうか。</summary>
		private static bool enabled;
		#endregion

		#region constructors
		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static PeriodicMessageCheckService() {
			MessageService.OnMessagePosted += (_, __) => ForceToCheck();
		}
		#endregion

		#region events
		/// <summary>
		/// 新着メッセージ発見イベント。
		/// </summary>
		public static event EventHandler? OnNewMessagePosted;
		#endregion

		#region properties
		/// <summary>
		/// 定期実行処理を稼働させるかどうか。
		/// </summary>
		public static bool Enabled {
			get => enabled;
			set {
				if (enabled == value) return;

				enabled = value;
				if (value) {
					CheckPeriodicallyAsync();
				}
			}
		}

		/// <summary>
		/// 取得完了しているMessageIDの最大値。
		/// </summary>
		public static int MaxMessageId { get; set; } = -1;
		#endregion

		#region private methods
		/// <summary>
		/// 強制的に新着メッセージ確認処理を実行する。
		/// </summary>
		private static void ForceToCheck() {
			CheckNewMessages();
		}

		/// <summary>
		/// 新着メッセージ確認処理。
		/// </summary>
		private static void CheckNewMessages() {
			var dao = new MessageDAO();
			if (ChannelService.CurrentChannel != null) {
				var id = dao.SelectMaxMessageId(ChannelService.CurrentChannel);

				if (MaxMessageId < id) {
					MaxMessageId = id;
					OnNewMessagePosted?.Invoke(null, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// 定期的に新着メッセージ確認処理を実行する。
		/// </summary>
		private static async void CheckPeriodicallyAsync() {
			while (Enabled) {
				await Task.Delay(MessageCheckIntervalSeconds).ConfigureAwait(false);
				CheckNewMessages();
			}
		}
		#endregion

	}
}

