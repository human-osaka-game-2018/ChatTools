using System.Collections.Generic;
using System.Data;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// チャンネル情報クラス。
	/// </summary>
	public class Channel {

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ユーザID</param>
		public Channel(int id) {
			this.Id = id;
		}
		#endregion

		#region parameters
		/// <summary>
		/// チャンネルID。
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// チャンネル名。
		/// </summary>
		public string ChannelName { get; set; } = string.Empty;
		#endregion

		#region static public methods
		/// <summary>
		/// <see cref="DataTable"/> を <see cref="Channel"/> の <see cref="List{T}"/> に変換する。
		/// </summary>
		/// <param name="dt">変換元</param>
		/// <returns>変換したオブジェクト</returns>
		public static List<Channel> ConvertFrom(DataTable dt) {
			var ret = new List<Channel>();
			foreach (var dr in dt.AsEnumerable()) {
				var channel = new Channel((int)dr["id"]);
				channel.ChannelName = dr["channel_name"].ToString()!;

				ret.Add(channel);
			}

			return ret;
		}
		#endregion
	}
}
