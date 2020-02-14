using System.Data;
using MySql.Data.MySqlClient;

namespace ChatTool.Infrastructure.Database {
	/// <summary>
	/// リアクションマスタにアクセスするクラス。
	/// </summary>
	public class ReactionDAO {

		/// <summary>テーブル名。</summary>
		private const string TableName = "m_reaction";

		/// <summary>
		/// SELECTを行う。
		/// </summary>
		/// <returns>取得したリアクション情報一覧</returns>
		public DataTable Select() {
			string sql = $"SELECT * FROM {TableName} WHERE is_deleted = 0;";

			using var con = Connection.Create();
			using var cmd = con.CreateCommand();
			cmd.CommandText = sql;

			using var da = new MySqlDataAdapter(cmd);
			var dt = new DataTable();
			da.Fill(dt);

			return dt;
		}

	}
}

