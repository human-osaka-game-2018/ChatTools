using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
		public Message(Channel channel, User user, string text) {
			this.Channel = channel;
			this.User = user;
			this.Text = text;
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
		public static List<Message> ConvertFrom(DataTable dt, Channel channel, List<User> users, Message? parentMessage = null) {
			var ret = new List<Message>();
			foreach (var dr in dt.AsEnumerable()) {
				var message = new Message((int)dr["id"]) {
					Channel = channel,
					User = users.First(x => x.Id == dr.Field<int>("user_id")),
					Text = dr.Field<string>("text"),
					PostedDate = dr.Field<DateTime>("time"),
					ParentMessage = (dr.Field<int?>("parent_message_id") == parentMessage?.Id) ? parentMessage : null
				};

				ret.Add(message);
			}

			return ret;
		}
		#endregion

	}
}
