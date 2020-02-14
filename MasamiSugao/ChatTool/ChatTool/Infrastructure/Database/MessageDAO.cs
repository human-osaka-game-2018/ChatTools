using System.Collections.Generic;
using System.Data;
using System.Text;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Extentions;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// メッセージ情報テーブルにアクセスするクラス。
	/// </summary>
	public class MessageDAO {

		#region constants
		/// <summary>メッセージテーブル名。</summary>
		private const string MessageTableName = "t_message";
		#endregion

		#region public methods
		/// <summary>
		/// <paramref name="channel"/> に表示するメッセージ一覧を取得する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="parentMessage">スレッドのメッセージを取得する場合は親メッセージを設定</param>
		/// <param name="users">ユーザ情報一覧</param>
		/// <returns>取得したメッセージ情報一覧</returns>
		public DataTable SelectMessages(Channel channel, Message? parentMessage, List<User> users) {
			// SQL組み立て
			StringBuilder sql = new StringBuilder();
			sql.Append($"SELECT * FROM {MessageTableName} ");
			sql.Append("WHERE channel_id = @channel_id AND is_deleted = 0 ");

			if (parentMessage == null) {
				sql.Append("AND displays_to_channel = 1 ");
			} else {
				sql.Append("AND (id = @parent_message_id OR parent_message_id = @parent_message_id) ");
			}

			sql.Append("ORDER BY time, id;");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			// パラメータ生成
			cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
			if (parentMessage != null) {
				cmd.Parameters.Add("@parent_message_id", MySqlDbType.Int32);
			}
			cmd.Prepare();

			// パラメータに値設定
			cmd.Parameters["@channel_id"].Value = channel.Id;
			if (parentMessage != null) {
				cmd.Parameters["@parent_message_id"].Value = parentMessage.Id;
			}

			// SQL実行
			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			return dt;
		}

		/// <summary>
		/// <paramref name="channel"/> に表示するメッセージのIDとReactionLogIDの最大値、レコード数を取得する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <returns>メッセージごとのReactionLogIDの最大値と数</returns>
		public DataTable SelectMaxReactionIds(Channel channel) {
			return this.SelectMaxReactionIds(channel, null);
		}

		/// <summary>
		/// 指定されたスレッドに表示するメッセージのIDとReactionLogIDの最大値、レコード数を取得する。
		/// </summary>
		/// <param name="parentMessage">対象スレッドの親メッセージID</param>
		/// <returns>メッセージごとのReactionLogIDの最大値と数</returns>
		public DataTable SelectMaxReactionIds(Message parentMessage) {
			return this.SelectMaxReactionIds(null, parentMessage);
		}

		/// <summary>
		/// INSERTを実行する。
		/// </summary>
		/// <param name="target">INSERT対象</param>
		/// <returns>成否</returns>
		public bool Insert(Message target) {
			// SQL組み立て
			StringBuilder sql = new StringBuilder();
			sql.Append($"INSERT INTO {MessageTableName} ( ");
			sql.Append("channel_id, user_id, text, time, parent_message_id, displays_to_channel");
			sql.Append(") values ( ");
			sql.Append("@channel_id, @user_id, @text, @time, @parent_message_id, @displays_to_channel");
			sql.Append(");");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			// パラメータ生成
			cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@user_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@text", MySqlDbType.VarChar, 280);
			cmd.Parameters.Add("@time", MySqlDbType.DateTime);
			cmd.Parameters.Add("@parent_message_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@displays_to_channel", MySqlDbType.Bit);
			cmd.Prepare();

			// パラメータに値設定
			cmd.Parameters["@channel_id"].Value = target.Channel!.Id;
			cmd.Parameters["@user_id"].Value = target.User!.Id;
			cmd.Parameters["@text"].Value = target.Text;
			cmd.Parameters["@time"].Value = target.PostedDate;
			cmd.Parameters["@parent_message_id"].Value = target.ParentMessage?.Id;
			cmd.Parameters["@displays_to_channel"].Value = target.DisplaysToChannel.ToInt();

			// SQL実行
			using var tran = con.BeginTransaction();
			var isSuccess = (cmd.ExecuteNonQuery() > 0);
			if (isSuccess) {
				tran.Commit();
			} else {
				tran.Rollback();
			}

			return isSuccess;
		}
		#endregion

		#region private methods
		/// <summary>
		/// 指定されたチャンネル又はスレッドに表示するメッセージのIDとReactionLogIDの最大値、レコード数を取得する。
		/// </summary>
		/// <param name="channel">対象チャンネル</param>
		/// <param name="parentMessage">対象スレッドの親メッセージID</param>
		/// <returns>メッセージごとのReactionLogIDの最大値と数</returns>
		/// <remarks><paramref name="channel"/>, <paramref name="parentMessage"/>のどちらかに値を設定すること</remarks>
		private DataTable SelectMaxReactionIds(Channel? channel, Message? parentMessage) {
			// SQL組み立て
			StringBuilder sql = new StringBuilder();
			sql.Append("SELECT ");
			sql.Append("message.id AS message_id, ");
			sql.Append("MAX(reactionLog.id) AS max_reaction_log_id, ");
			sql.Append("COUNT(reactionLog.id) AS reaction_log_count ");

			sql.Append($"FROM {MessageTableName} AS message ");
			sql.Append($"LEFT OUTER JOIN {ReactionLogDAO.ReactionLogTableName} AS reactionLog ");
			sql.Append("ON message.ID = reactionLog.message_id ");

			sql.Append("WHERE ");

			if (channel != null) {
				sql.Append("message.channel_id = @channel_id ");
				sql.Append("AND message.displays_to_channel = 1 ");
			} else {
				sql.Append("message.id = @parent_message_id OR message.parent_message_id = @parent_message_id ");
			}
			sql.Append("AND message.is_deleted = 0 ");
			sql.Append("GROUP BY message.id; ");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			// パラメータ生成 & 値設定
			if (channel != null) {
				cmd.Parameters.Add("@channel_id", MySqlDbType.Int32);
				cmd.Parameters["@channel_id"].Value = channel.Id;
			} else {
				cmd.Parameters.Add("@parent_message_id", MySqlDbType.Int32);
				cmd.Parameters["@parent_message_id"].Value = parentMessage!.Id;
			}
			cmd.Prepare();

			// SQL実行
			using var da = new MySqlDataAdapter(cmd);
			using var dt = new DataTable();
			da.Fill(dt);

			return dt;
		}
		#endregion

	}
}

