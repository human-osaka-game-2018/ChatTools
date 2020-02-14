using System.Data;
using System.Text;
using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// チャンネルマスタにアクセスするクラス。
	/// </summary>
	public class ChannelDAO {

		#region constants
		/// <summary>チャンネルテーブル名。</summary>
		private const string ChannelTableName = "m_channel";

		/// <summary>チャンネルメンバーテーブル名。</summary>
		private const string ChannelMemberTableName = "m_channel_member";
		#endregion

		#region public methods
		/// <summary>
		/// <paramref name="user"/> が所属するチャンネ一覧を取得する。
		/// </summary>
		/// <param name="user">検索条件となるユーザ</param>
		/// <returns>取得したチャンネル情報一覧</returns>
		public DataTable SelectBelongingChannels(User user) {
			StringBuilder sql = new StringBuilder();
			sql.Append($"SELECT * FROM {ChannelTableName} AS ch ");
			sql.Append($"INNER JOIN {ChannelMemberTableName} as member ");
			sql.Append("ON ch.ID = member.channel_id ");
			sql.Append("WHERE member.user_id = @user_id AND is_deleted = 0 ");
			sql.Append("ORDER BY ch.id;");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			cmd.Parameters.Add("@user_id", MySqlDbType.Int32);
			cmd.Prepare();
			cmd.Parameters["@user_id"].Value = user.Id;

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			return dt;
		}
		#endregion

	}
}

