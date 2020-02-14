using System.Collections.Generic;
using System.Data;
using System.Text;
using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// リアクションログテーブルにアクセスするクラス。
	/// </summary>
	public class ReactionLogDAO {

		#region constants
		/// <summary>リアクションログテーブル名。</summary>
		public const string ReactionLogTableName = "t_reaction_log";
		#endregion

		#region public methods
		/// <summary>
		/// <paramref name="messages"/> のリアクションログを取得する。
		/// </summary>
		/// <param name="messages">対象メッセージ</param>
		/// <returns>取得したリアクションログ一覧</returns>
		public DataTable Select(List<Message> messages) {
			using var con = Connection.Create();
			using var cmd = con.CreateCommand();

			StringBuilder sql = new StringBuilder();
			sql.Append($"SELECT * FROM {ReactionLogTableName} ");
			sql.Append("WHERE message_id IN ( ");

			for (int i = 0; i < messages.Count; i++) {
				var parameterName = "@message_id_" + i;
				sql.Append(parameterName);

				if (i < messages.Count - 1) {
					sql.Append(", ");
				}

				cmd.Parameters.Add(parameterName, MySqlDbType.Int32);
				cmd.Parameters[parameterName].Value = messages[i].Id;
			}

			sql.Append(") ");
			sql.Append("ORDER BY id;");

			cmd.CommandText = sql.ToString();
			cmd.Prepare();

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			return dt;
		}

		/// <summary>
		/// リアクションログテーブルにデータを追加する。
		/// </summary>
		/// <param name="message">メッセージ情報</param>
		/// <param name="reaction">リアクション情報</param>
		/// <param name="user">ユーザ</param>
		/// <returns>成否</returns>
		public bool Insert(Message message, Reaction reaction, User user) {
			// SQL組み立て
			StringBuilder sql = new StringBuilder();
			sql.Append($"INSERT INTO {ReactionLogTableName} (");
			sql.Append("message_id, user_id, reaction_id) VALUES (");
			sql.Append("@message_id, @user_id, @reaction_id);");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			// パラメータ生成
			cmd.Parameters.Add("@message_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@user_id", MySqlDbType.Int32);
			cmd.Parameters.Add("@reaction_id", MySqlDbType.Int32);
			cmd.Prepare();
			// パラメータに値設定
			cmd.Parameters["@message_id"].Value = message.Id;
			cmd.Parameters["@user_id"].Value = user.Id;
			cmd.Parameters["@reaction_id"].Value = reaction.Id;

			// SQL実行
			using var tran = con.BeginTransaction();
			var ret = (cmd.ExecuteNonQuery() > 0);
			if (ret) {
				tran.Commit();
			} else {
				tran.Rollback();
			}

			return ret;
		}

		/// <summary>
		/// リアクションログテーブルからデータを削除する。
		/// </summary>
		/// <param name="reactionLog">削除対象データ</param>
		/// <returns>成否</returns>
		public bool Delete(ReactionLog reactionLog) {
			StringBuilder sql = new StringBuilder();
			sql.Append($"DELETE FROM {ReactionLogTableName} WHERE ");
			sql.Append("id = @id;");

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();

			cmd.Parameters.Add("@id", MySqlDbType.Int32);
			cmd.Prepare();
			cmd.Parameters["@id"].Value = reactionLog.Id;

			using var tran = con.BeginTransaction();
			var ret = (cmd.ExecuteNonQuery() > 0);
			if (ret) {
				tran.Commit();
			} else {
				tran.Rollback();
			}

			return ret;
		}
		#endregion

	}
}

