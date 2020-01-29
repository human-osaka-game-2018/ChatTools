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
			sql.Append("WHERE channel_id = @channel_id AND displays_to_channel = 1 AND is_deleted = 0 ");
			sql.Append("ORDER BY time, id;");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
			cmd.Prepare();
			cmd.Parameters["@channel_id"].Value = channel.Id;

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			var ret = Message.ConvertFrom(dt, channel, users);

			return ret;
		}

		/// <summary>
		/// INSERTを実行する。
		/// </summary>
		/// <param name="target">INSERT対象</param>
		/// <returns>成否</returns>
		public bool Insert(Message target) {
			StringBuilder sql = new StringBuilder();
			sql.Append($"INSERT INTO {MessageTableName} ( ");
			sql.Append("channel_id, user_id, text, time, parent_message_id");
			sql.Append(") values ( ");
			sql.Append("@channel_id, @user_id, @text, @time, @parent_message_id");
			sql.Append(");");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@user_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@text", MySqlDbType.VarChar, 280);
			cmd.Parameters.Add("@time", MySqlDbType.DateTime);
			cmd.Parameters.Add("@parent_message_id", MySqlDbType.Int32);
			cmd.Prepare();
			cmd.Parameters["@channel_id"].Value = target.Channel!.Id;
			cmd.Parameters["@user_id"].Value = target.User!.Id;
			cmd.Parameters["@text"].Value = target.Text;
			cmd.Parameters["@time"].Value = target.PostedDate;
			cmd.Parameters["@parent_message_id"].Value = target.ParentMessage?.Id;

			var isSuccess = (cmd.ExecuteNonQuery() > 0);

			return isSuccess;
		}
		#endregion

	}
}
