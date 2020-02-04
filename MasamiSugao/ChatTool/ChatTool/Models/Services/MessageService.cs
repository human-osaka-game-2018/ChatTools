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
		/// <summary>
		/// 現在選択中のメッセージ。
		/// </summary>
		private static Message? selectedMessage;
		#endregion

		#region events
		/// <summary>
		/// メッセージ選択イベント。
		/// </summary>
		public static event EventHandler<Message?>? OnMessageSelected;

		/// <summary>
		/// メッセージ投稿イベント。
		/// </summary>
		public static event EventHandler? OnMessagePosted;
		#endregion

		#region properties
		/// <summary>
		/// 現在選択中のメッセージ。
		/// </summary>
		public static Message? SelectedMessage {
			get => selectedMessage;
			set {
				if (selectedMessage == value) return;

				selectedMessage = value;
				OnMessageSelected?.Invoke(null, value);
			}
		}
		#endregion

		#region public methods
		/// <summary>
		/// メッセージ一覧データを生成する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <returns>一覧データ</returns>
		public List<Message> ListMessagesBy(Channel channel) {
			var userDAO = new UserDAO();
			var messageDAO = new MessageDAO();
			var messages = messageDAO.SelectMessages(channel, userDAO.SelectAll());

			// 新着メッセージチェック処理に現在の最大IDを知らせる
			PeriodicMessageCheckService.MaxMessageId = (messages.Count == 0) ? -1 : messages.Max(x => x.Id!.Value);

			return messages;
		}

		/// <summary>
		/// メッセージ一覧データを取得し、既存のリストより新しいものを末尾に追加する。
		/// </summary>
		/// <param name="target">追加対象のリスト</param>
		/// <param name="channel">対象チャンネル</param>
		public void AddNewerMessagesTo(ObservableCollection<Message> target, Channel channel) {
			// 最新のユーザ情報も取得しておく
			var userDAO = new UserDAO();
			var messageDAO = new MessageDAO();
			var messages = messageDAO.SelectMessages(channel, userDAO.SelectAll());

			// IDが大きいもののみ追加
			messages.Where(x => x.Id > target.Max(y => y.Id)).ToList().ForEach(x => target.Add(x));

			// 新着メッセージチェック処理に現在の最大IDを知らせる
			PeriodicMessageCheckService.MaxMessageId = (messages.Count == 0) ? -1 : messages.Max(x => x.Id!.Value);
		}

		/// <summary>
		/// メッセージを投稿する。
		/// </summary>
		/// <param name="message">投稿メッセージ</param>
		public void Post(Message message) {
			var messageDAO = new MessageDAO();
			var isSuccess =  messageDAO.Insert(message);

			if (isSuccess) {
				OnMessagePosted?.Invoke(null, EventArgs.Empty);
			}
		}
		#endregion

	}
}

