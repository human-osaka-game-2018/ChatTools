using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ChatTool.Models.Services;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// リアクション情報クラス。
	/// </summary>
	public class ReactionLogCollection {

		#region constants
		/// <summary>二人称を示す文字列。</summary>
		private const string SecondPersonPronoun = "あなた";

		/// <summary>継承フォーマット。</summary>
		private const string PolitteFormatOfName = "{0} さん";

		/// <summary>リアクションしたユーザの説明文のフォーマット。</summary>
		private const string UsersInfoMessageFormat = "{0}がリアクションしました";

		/// <summary>ユーザ名を区切るデリミタ。</summary>
		private const string DelimiterForUsers = ", ";
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="reaction">リアクション</param>
		/// <param name="message">メッセージ</param>
		/// <param name="users">ReactionLogIDとユーザ一覧</param>
		public ReactionLogCollection(Reaction reaction, Message message, List<ReactionLog>? users = null) {
			this.Reaction = reaction;
			this.Users = users ?? new List<ReactionLog>();

			// リアクション追加、削除時のイベント設定
			this.OnReactionAdded += (_, reaction) => MessageService.AddReaction(message, reaction);
			this.OnReactionRemoved += (_, reactionLog) => MessageService.RemoveReaction(message, reactionLog);
		}
		#endregion

		#region event
		/// <summary>
		/// リアクション追加イベント。
		/// </summary>
		public event EventHandler<Reaction>? OnReactionAdded;

		/// <summary>
		/// リアクション削除イベント。
		/// </summary>
		public event EventHandler<ReactionLog>? OnReactionRemoved;
		#endregion

		#region properties
		/// <summary>
		/// リアクション。
		/// </summary>
		public Reaction Reaction { get; }

		/// <summary>
		/// ReactionLogIDとユーザの一覧。
		/// </summary>
		public List<ReactionLog> Users { get; }

		/// <summary>
		/// ユーザ数。
		/// </summary>
		public int UserCount => this.Users.Count;

		/// <summary>
		/// リアクションしたユーザ情報を知らせるメッセージ文面。
		/// </summary>
		public string UsersInfoMessage => string.Format(
											UsersInfoMessageFormat,
											this.Users
												.Select(x => x.User.IsCurrentUser ? SecondPersonPronoun : string.Format(PolitteFormatOfName, x.User.UserName))
												.Aggregate((x, y) => x + DelimiterForUsers + y));

		/// <summary>
		/// ログインユーザがリアクション選択中かどうか。
		/// </summary>
		public bool IsSelected {
			get => this.Users.Exists(x => x.User == LoginService.CurrentUser!);
			set {
				var user = this.Users.FirstOrDefault(x => x.User == LoginService.CurrentUser!);
				switch (value) {
					case true when user == null:
						this.OnReactionAdded?.Invoke(this, this.Reaction);
						break;
					case false when user != null:
						this.OnReactionRemoved?.Invoke(this, user);
						break;
				}
			}
		}
		#endregion

		#region public static methods
		/// <summary>
		/// <paramref name="messages"/> にリアクション情報を追加する。
		/// </summary>
		/// <param name="messages">追加対象のメッセージ一覧</param>
		/// <param name="reactionLogDT">リアクションログ</param>
		/// <param name="users">ユーザ一覧</param>
		public static void AddTo(List<Message> messages, DataTable reactionLogDT, List<User> users) {
			messages.ForEach(message => {
				// リアクション情報のDataTableをReactionLogCollection型のリストに変換
				var reactionLogCollections = reactionLogDT.AsEnumerable()
					.Where(dr => dr.Field<int>("message_id") == message.Id)
					.GroupBy(x => x.Field<int>("reaction_id"))
					.Select((x, _) => {
						var reaction = Reaction.Get(x.Key);
						var reactionLog = new ReactionLogCollection(
							reaction,
							message,
							x.AsEnumerable().Select(dr => new ReactionLog(dr.Field<int>("id"), users.First(y => y.Id == dr.Field<int>("user_id")), reaction)).ToList());

						return reactionLog;
					});

				// メッセージに現在設定されているリアクション情報をいったん全削除してから追加
				message.ReactionLogs.Clear();
				reactionLogCollections.ToList().ForEach(x => message.ReactionLogs.Add(x));
			});
		}
		#endregion

	}
}

