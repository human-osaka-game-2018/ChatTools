﻿using System.Collections.Generic;
using System.Data;
using System.Text;
using ChatTool.Models.DomainObjects;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// ユーザ情報テーブルにアクセスするクラス。
	/// </summary>
	public class UserDAO {

		#region constants
		/// <summary>
		/// テーブル名。
		/// </summary>
		private const string TableName = "m_user";
		#endregion

		#region public methods
		/// <summary>
		/// SELECTを行う。
		/// </summary>
		/// <param name="mailAddress">検索条件となるメールアドレス</param>
		/// <returns>取得したユーザ情報一覧</returns>
		public List<User> Select(string mailAddress) {
			string sql = $"SELECT * FROM {TableName} WHERE mail_address = @mail_address;";

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql;
			cmd.Parameters.Add("@mail_address", MySqlDbType.VarChar, 256);
			cmd.Prepare();
			cmd.Parameters["@mail_address"].Value = mailAddress;

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			var ret = User.ConvertFrom(dt);

			return ret;
		}

		/// <summary>
		/// オンラインフラグを更新する。
		/// </summary>
		/// <param name="user">更新対象のユーザ</param>
		/// <param name="isOnline">更新する値</param>
		/// <returns>成否</returns>
		public bool UpdateOnlineFlag(User user, bool isOnline) {
			// SQL組み立て
			var sql = new StringBuilder();
			sql.Append("UPDATE ").Append(TableName).Append(" SET ");
			sql.Append("is_online = @is_online ");
			sql.Append("WHERE id = @id;");

			// パラメータ生成
			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql.ToString();
			cmd.Parameters.Add("@is_online", MySqlDbType.Bit);
			cmd.Parameters.Add("@id", MySqlDbType.Int32);
			cmd.Prepare();

			// パラメータに値設定
			cmd.Parameters["@is_online"].Value = isOnline ? 1 : 0;
			cmd.Parameters["@id"].Value = user.Id;

			// SQL実行
			var ret = (cmd.ExecuteNonQuery() > 0);

			return ret;
		}
		#endregion

	}
}
