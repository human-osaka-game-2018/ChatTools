using System.Collections.Generic;
using System.Data;
using System.Linq;

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
			var channels = dt.AsEnumerable().Select(dr =>
				new Channel(dr.Field<int>("id")) { ChannelName = dr.Field<string>("channel_name") });

			return channels.ToList();
		}
		#endregion

	}
}

