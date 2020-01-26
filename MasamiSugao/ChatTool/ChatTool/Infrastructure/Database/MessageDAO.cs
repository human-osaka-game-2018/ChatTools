using System.Collections.Generic;
using System.Data;
using System.Text;
using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// メッセージ情報テーブルにアクセスするクラス。
	/// </summary>
	public class MessageDAO {

		#region constants
		/// <summary>
		/// チャンネルテーブル名。
		/// </summary>
		private const string MessageTableName = "t_message";
		#endregion

		#region public methods
		/// <summary>
		/// <paramref name="channel"/> に表示するメッセージ一覧を取得する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="users">ユーザ情報一覧</param>
		/// <returns>取得したメッセージ情報一覧</returns>
		public List<Message> SelectMessages(Channel channel, List<User> users) {
			StringBuilder sql = new StringBuilder();
			sql.Append($"SELECT * FROM {MessageTableName} ");
			sql.Append("WHERE channel_id = @channel_id AND displays_to_channel = 1 AND is_deleted = 0;");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
			cmd.Prepare();
			cmd.Parameters["@channel_id"].Value = channel.Id;

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			var ret = Message.ConvertFrom(dt, users);

			return ret;
		}
		#endregion

	}
}
