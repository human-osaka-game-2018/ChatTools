using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Models.Services {
	/// <summary>
	/// メッセージに関するサービスクラス。
	/// </summary>
	public class MessageService {

		#region field members
		/// <summary>選択中のスレッドの親メッセージ。</summary>
		private static Message? parentOfcurrentMessageThread;
		#endregion

		#region constructors
		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static MessageService() {
#if DEBUG
			// デザイナから実行時はDBアクセス処理を呼び出さないようにする
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				return;
			}
#endif

			// リアクション一覧データを生成しておく
			var dao = new ReactionDAO();
			var dt = dao.Select();
			Reaction.ConvertFrom(dt);
		}
		#endregion

		#region events
		/// <summary>
		/// メッセージ選択イベント。
		/// </summary>
		public static event EventHandler<Message?>? OnMessageThreadChanged;

		/// <summary>
		/// メッセージ投稿イベント。
		/// </summary>
		public static event EventHandler<Message>? OnMessagePosted;

		/// <summary>
		/// リアクション変更イベント。
		/// </summary>
		public static event EventHandler<Message>? OnReactionChanged;
		#endregion

		#region properties
		/// <summary>
		/// 選択中のスレッドの親メッセージ。
		/// </summary>
		public static Message? ParentOfCurrentMessageThread {
			get => parentOfcurrentMessageThread;
			set {
				if (parentOfcurrentMessageThread == value) return;

				parentOfcurrentMessageThread = value;
				OnMessageThreadChanged?.Invoke(null, value);
			}
		}
		#endregion

		#region public static methods
		/// <summary>
		/// リアクションを追加する。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="reaction">リアクション</param>
		public static void AddReaction(Message message, Reaction reaction) {
			var reactionLogDAO = new ReactionLogDAO();
			var isSuccess = reactionLogDAO.Insert(message, reaction, LoginService.CurrentUser!);

			if (isSuccess) {
				OnReactionChanged?.Invoke(null, message);
			}
		}

		/// <summary>
		/// リアクションを削除する。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="reactionLog">リアクションログ</param>
		public static void RemoveReaction(Message message, ReactionLog reactionLog) {
			var reactionLogDAO = new ReactionLogDAO();
			var isSuccess = reactionLogDAO.Delete(reactionLog);

			if (isSuccess) {
				OnReactionChanged?.Invoke(null, message);
			}
		}
		#endregion

		#region public methods
		/// <summary>
		/// メッセージ一覧データを生成する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="parentMessage">スレッドのデータを取得する場合は親メッセージを設定</param>
		/// <returns>一覧データ</returns>
		public List<Message> ListMessagesBy(Channel channel, Message? parentMessage = null) {
			// 最新のユーザ情報も取得しておく
			var userDAO = new UserDAO();
			var userDT = userDAO.SelectAll();
			var users = User.ConvertFrom(userDT);

			var messageDAO = new MessageDAO();
			var messageDT = messageDAO.SelectMessages(channel, parentMessage, users);
			var messages = Message.ConvertFrom(messageDT, channel, users);

			if (messages.Count > 0) {
				var reactionLogDAO = new ReactionLogDAO();
				var reactionLogDT = reactionLogDAO.Select(messages);
				ReactionLogCollection.AddTo(messages, reactionLogDT, users);
			}

			return messages;
		}

		/// <summary>
		/// メッセージ一覧データを取得し、既存のリストより新しいものを末尾に追加する。
		/// </summary>
		/// <param name="target">追加対象のリスト</param>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="parentMessage">スレッドのデータを取得する場合は親メッセージを設定</param>
		public void AddNewerMessagesTo(ObservableCollection<Message> target, Channel channel, Message? parentMessage = null) {
			var messages = this.ListMessagesBy(channel, parentMessage);

			// リアクションログに変更があるかチェック
			var reactionChangedMessages = target.Where(x => {
				var message = messages.FirstOrDefault(y => y.Id == x.Id);
				var ret = (message == null);
				ret = ret || (x.ReactionLogs.Count != message!.ReactionLogs.Count);
				ret = ret || x.ReactionLogs.Any(y => {
					// ReactionLogIDの最大値とレコード数が同じかチェック
					var reaction = message!.ReactionLogs.FirstOrDefault(z => z.Reaction == y.Reaction);
					return ((reaction == null) || (y.Users.Max(z => z.Id) != reaction.Users.Max(z => z.Id)) || (y.Users.Count != reaction.Users.Count));
				});
				return ret;
			}).ToList();

			// リアクションログに変更があるメッセージはリアクションログの入れ替え
			reactionChangedMessages.ForEach(x => {
				x.ReactionLogs.Clear();
				var message = messages.FirstOrDefault(y => y.Id == x.Id);
				message?.ReactionLogs.ToList().ForEach(y => x.ReactionLogs.Add(y));
			});

			// IDが大きいもメッセージのみ追加
			messages.Where(x => x.Id > target.Max(y => y.Id)).ToList().ForEach(x => target.Add(x));
		}

		/// <summary>
		/// メッセージを投稿する。
		/// </summary>
		/// <param name="message">投稿メッセージ</param>
		public void Post(Message message) {
			var messageDAO = new MessageDAO();
			var isSuccess =  messageDAO.Insert(message);

			if (isSuccess) {
				OnMessagePosted?.Invoke(null, message);
			}
		}
		#endregion

	}
}

