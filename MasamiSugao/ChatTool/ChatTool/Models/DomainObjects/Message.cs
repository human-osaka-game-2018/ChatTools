using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using ChatTool.Models.Extentions;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// メッセージ情報クラス。
	/// </summary>
	public class Message : EntityBase {

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="channel">投稿チャンネル</param>
		/// <param name="user">投稿ユーザ</param>
		/// <param name="text">メッセージ文</param>
		/// <param name="parentMessage">親メッセージ</param>
		/// <param name="displaysToChannel">チャンネルに表示するかどうか</param>
		public Message(Channel channel, User user, string text, bool displaysToChannel = true, Message? parentMessage = null)
				: this(-1, channel, user, text, displaysToChannel, parentMessage) {
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">メッセージID</param>
		/// <param name="channel">投稿チャンネル</param>
		/// <param name="user">投稿ユーザ</param>
		/// <param name="text">メッセージ文</param>
		/// <param name="displaysToChannel">チャンネルに表示するかどうか</param>
		/// <param name="parentMessage">親メッセージ</param>
		private Message(int id, Channel channel, User user, string text, bool displaysToChannel = true, Message? parentMessage = null) : base(id) {
			this.Channel = channel;
			this.User = user;
			this.Text = text;
			this.ParentMessage = parentMessage;
			this.DisplaysToChannel = displaysToChannel;
		}
		#endregion

		#region parameters
		/// <summary>
		/// 投稿チャンネル。
		/// </summary>
		public Channel? Channel { get; }

		/// <summary>
		/// 投稿ユーザ。
		/// </summary>
		public User? User { get; }

		/// <summary>
		/// メッセージ文。
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// 投稿日。
		/// </summary>
		public DateTime PostedDate { get; set; } = DateTime.Now;

		/// <summary>
		/// スレッドの親メッセージ。
		/// </summary>
		/// <remarks>スレッドの子メッセージの場合のみ格納される。</remarks>
		public Message? ParentMessage { get; private set; } = null;

		/// <summary>
		/// チャンネルに表示するかどうか。
		/// </summary>
		public bool DisplaysToChannel { get; }

		/// <summary>
		/// リアクション一覧。
		/// </summary>
		public ObservableCollection<ReactionLogCollection> ReactionLogs { get; } = new ObservableCollection<ReactionLogCollection>();
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
			var message = new Message(
				dr.Field<int>("id"),
				channel,
				users.First(x => x.Id == dr.Field<int>("user_id")),
				dr.Field<string>("text"),
				dr.Field<sbyte>("displays_to_channel").ToBool()
			) { PostedDate = dr.Field<DateTime>("time") };

			return message;
		}
		#endregion

		#region public methods
		/// <summary>
		/// メッセージへのリアクションを追加する。
		/// </summary>
		/// <param name="reaction">追加するリアクションの種類</param>
		public void AddReaction(Reaction reaction) {
			var target = this.ReactionLogs.FirstOrDefault(x => x.Reaction == reaction) ??
				new ReactionLogCollection(reaction, this);
			// リアクション選択状態に変更
			target.IsSelected = true;
		}
		#endregion

	}
}

