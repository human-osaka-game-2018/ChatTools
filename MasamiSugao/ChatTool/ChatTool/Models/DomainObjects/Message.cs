using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ChatTool.Models.Extentions;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// メッセージ情報クラス。
	/// </summary>
	public class Message {

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">メッセージID</param>
		public Message(int id) {
			this.Id = id;
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="channel">投稿チャンネル</param>
		/// <param name="user">投稿ユーザ</param>
		/// <param name="text">メッセージ文</param>
		/// <param name="parentMessage">親メッセージ</param>
		/// <param name="displaysToChannel">チャンネルに表示するかどうか</param>
		public Message(Channel channel, User user, string text, Message? parentMessage = null, bool displaysToChannel = true) {
			this.Channel = channel;
			this.User = user;
			this.Text = text;
			this.ParentMessage = parentMessage;
			this.DisplaysToChannel = displaysToChannel;
		}
		#endregion

		#region parameters
		/// <summary>
		/// メッセージID。
		/// </summary>
		public int? Id { get; }

		/// <summary>
		/// 投稿チャンネル。
		/// </summary>
		public Channel? Channel { get; set; }

		/// <summary>
		/// 投稿ユーザ。
		/// </summary>
		public User? User { get; set; }

		/// <summary>
		/// メッセージ文。
		/// </summary>
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// 投稿日。
		/// </summary>
		public DateTime PostedDate { get; set; } = DateTime.Now;

		/// <summary>
		/// スレッドの親メッセージ。
		/// </summary>
		/// <remarks>スレッドの子メッセージの場合のみ格納される。</remarks>
		public Message? ParentMessage { get; set; } = null;

		/// <summary>
		/// チャンネルに表示するかどうか。
		/// </summary>
		public bool DisplaysToChannel { get; set; } = true;
		#endregion

		#region static public methods
		/// <summary>
		/// <see cref="DataTable"/> を <see cref="Message"/> の <see cref="List{T}"/> に変換する。
		/// </summary>
		/// <param name="dt">変換元</param>
		/// <param name="channel">チャンネル</param>
		/// <param name="users">ユーザ情報一覧</param>
		/// <param name="parentMessage">親メッセージ情報</param>
		/// <returns>変換したオブジェクト</returns>
		public static List<Message> ConvertFrom(DataTable dt, Channel channel, List<User> users) {
			var messages = dt.AsEnumerable().Select(dr => {
				var message = ConvertFrom(dr, channel, users);

				// 親メッセージ
				var parentMessageId = dr.Field<int?>("parent_message_id");
				var parentMessageRow = (parentMessageId == null) ? null : dt.AsEnumerable().FirstOrDefault(x => x.Field<int>("id") == parentMessageId);
				message.ParentMessage = (parentMessageRow == null) ? null : ConvertFrom(parentMessageRow, channel, users);

				return message;
			});

			return messages.ToList();
		}
		#endregion

		#region static private methods
		/// <summary>
		/// <see cref="DataRow"/> を <see cref="Message"/> に変換する。
		/// </summary>
		/// <param name="dr">変換元</param>
		/// <param name="channel">チャンネル</param>
		/// <param name="users">ユーザ情報一覧</param>
		/// <returns>変換したオブジェクト</returns>
		private static Message ConvertFrom(DataRow dr, Channel channel, List<User> users) {
			var message = new Message(dr.Field<int>("id")) {
				Channel = channel,
				User = users.First(x => x.Id == dr.Field<int>("user_id")),
				Text = dr.Field<string>("text"),
				PostedDate = dr.Field<DateTime>("time"),
				DisplaysToChannel = dr.Field<sbyte>("displays_to_channel").ToBool()
			};

			return message;
		}
		#endregion

	}
}

