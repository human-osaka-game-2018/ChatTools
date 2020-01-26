using System;
using System.Collections.Generic;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Models.Services {
	/// <summary>
	/// チャンネルに関するサービスクラス。
	/// </summary>
	public static class MessageService {

		/// <summary>
		/// 現在選択中のメッセージ。
		/// </summary>
		private static Message? selectedMessage;

		/// <summary>
		/// メッセージ選択イベント。
		/// </summary>
		public static event EventHandler<Message?>? OnMessageSelected;

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

		/// <summary>
		/// メッセージ一覧データを生成する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <returns>一覧データ</returns>
		public static List<Message> ListMessagesBy(Channel channel) {
			var userDAO = new UserDAO();
			var messageDAO = new MessageDAO();
			return messageDAO.SelectMessages(channel, userDAO.SelectAll());
		}

	}
}
