using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Models.Services {
	/// <summary>
	/// メッセージに関するサービスクラス。
	/// </summary>
	public class MessageService {

		#region field members
		/// <summary>選択中のスレッドの親メッセージ。</summary>
		private static Message? currentMessageThreadParent;
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
		#endregion

		#region properties
		/// <summary>
		/// 選択中のスレッドの親メッセージ。
		/// </summary>
		public static Message? CurrentMessageThreadParent {
			get => currentMessageThreadParent;
			set {
				if (currentMessageThreadParent == value) return;

				currentMessageThreadParent = value;
				OnMessageThreadChanged?.Invoke(null, value);
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
			var userDAO = new UserDAO();
			var messageDAO = new MessageDAO();
			var messages = messageDAO.SelectMessages(channel, parentMessage, userDAO.SelectAll());

			return messages;
		}

		/// <summary>
		/// メッセージ一覧データを取得し、既存のリストより新しいものを末尾に追加する。
		/// </summary>
		/// <param name="target">追加対象のリスト</param>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="parentMessage">スレッドのデータを取得する場合は親メッセージを設定</param>
		public void AddNewerMessagesTo(ObservableCollection<Message> target, Channel channel, Message? parentMessage = null) {
			// 最新のユーザ情報も取得しておく
			var userDAO = new UserDAO();
			var messageDAO = new MessageDAO();
			var messages = messageDAO.SelectMessages(channel, parentMessage, userDAO.SelectAll());

			// IDが大きいもののみ追加
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

