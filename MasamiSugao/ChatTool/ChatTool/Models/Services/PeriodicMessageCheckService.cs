using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
			MessageService.OnReactionChanged += (_, __) => ForceToCheck();
		}
		#endregion

		#region events
		/// <summary>
		/// メッセージ又はリアクション変更検知イベント。
		/// </summary>
		public event EventHandler? OnMessageOrReactionModified;
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
		public int MaxMessageId => (this.ReactionLogVersionDic.Count == 0) ? -1 : this.ReactionLogVersionDic.Max(x => x.Value.MessageId);

		/// <summary>
		/// 取得完了しているメッセージごとのReactionLogIdの最大値とレコード数。
		/// </summary>
		public Dictionary<int, ReactionLogVersion> ReactionLogVersionDic { get; private set; } = new Dictionary<int, ReactionLogVersion>();
		#endregion

		#region public methods
		/// <summary>
		/// 取得完了したメッセージとReactionLogIDを設定する。
		/// </summary>
		/// <param name="reactionLogVersions">メッセージごとのReactionLogのバージョン情報</param>
		public void SetReactionLogVersions(IEnumerable<ReactionLogVersion> reactionLogVersions) {
			if (!reactionLogVersions.Any()) {
				this.ReactionLogVersionDic.Clear();
				return;
			}

			// 情報が更新されている場合はイベント発火
			if (this.MaxMessageId < reactionLogVersions.Max(x => x.MessageId) ||
					reactionLogVersions.Any(x => !this.ReactionLogVersionDic.ContainsKey(x.MessageId) ||
						this.ReactionLogVersionDic[x.MessageId].MaxReactionLogId != x.MaxReactionLogId ||
						this.ReactionLogVersionDic[x.MessageId].ReactionLogCount != x.ReactionLogCount)) {

				var dic = reactionLogVersions.ToDictionary(x => x.MessageId, x => x);
				this.ReactionLogVersionDic = dic;
				this.OnMessageOrReactionModified?.Invoke(null, EventArgs.Empty);
			}
		}
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
			if (ChannelService.CurrentChannel == null) return;

			var dt = this.ParentMessage switch {
				null => dao.SelectMaxReactionIds(ChannelService.CurrentChannel),
				_ => dao.SelectMaxReactionIds(this.ParentMessage)
			};

			// 取得した情報の保持と更新イベント発火
			var reactionLogVersions = dt.AsEnumerable().Select(dr => new ReactionLogVersion(
				dr.Field<int>("message_id"),
				dr.IsNull("max_reaction_log_id") ? -1 : dr.Field<int>("max_reaction_log_id"),
				Convert.ToInt32(dr.Field<long>("reaction_log_count"))));
			this.SetReactionLogVersions(reactionLogVersions);
		}

		/// <summary>
		/// 定期的に新着メッセージ確認処理を実行する。
		/// </summary>
		private async void CheckPeriodicallyAsync() {
			try {
				while (this.Enabled && !App.IsHandlingException) {
					CheckNewMessages();
					await Task.Delay(MessageCheckIntervalSeconds).ConfigureAwait(false);
				}
			} catch (Exception) {
				this.Enabled = false;
				// 例外処理中に更に例外を投げると例外通知のメッセージが表示されなくなるようなのでフラグで制御
				if (!App.IsHandlingException) {
					App.IsHandlingException = true;
					throw;
				}
			}
		}
		#endregion

		#region inner classes
		/// <summary>
		/// リアクションログのバージョン管理用の情報クラス。
		/// </summary>
		public class ReactionLogVersion {

			/// <summary>
			/// コンストラクタ。
			/// </summary>
			/// <param name="messageId">MessageID</param>
			/// <param name="maxReactionLogId">ReactionLogIDの最大値。存在しない場合は-1。</param>
			/// <param name="reactionLogCount">リアクションログのレコード数</param>
			public ReactionLogVersion(int messageId, int maxReactionLogId, int reactionLogCount) {
				this.MessageId = messageId;
				this.MaxReactionLogId = maxReactionLogId;
				this.ReactionLogCount = reactionLogCount;
			}

			/// <summary>
			/// MessageID.
			/// </summary>
			public int MessageId { get; }

			/// <summary>
			/// ReactionLogIDの最大値。
			/// </summary>
			public int MaxReactionLogId { get; }

			/// <summary>
			/// ReactionLogIDの最大値。
			/// </summary>
			public int ReactionLogCount { get; }

		}
		#endregion

	}
}

