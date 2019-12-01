using System.Collections.Generic;
using System.Data;
using System.Linq;
using ChatTool.Infrastructure.Database;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// ユーザ情報クラス。
	/// </summary>
	public class User {

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ユーザID</param>
		public User(int id) {
			this.Id = id;
		}
		#endregion

		#region parameters
		/// <summary>
		/// ユーザID。
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// ユーザ名。
		/// </summary>
		public string UserName { get; set; } = string.Empty;

		/// <summary>
		/// メールアドレス。
		/// </summary>
		public string MailAddress { get; set; } = string.Empty;

		/// <summary>
		/// パスワード。
		/// </summary>
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// アイコンID。
		/// </summary>
		public int IconId { get; }

		/// <summary>
		/// オンラインフラグ。
		/// </summary>
		public bool IsOnline { get; set; } = false;
		#endregion

		#region static public methods
		/// <summary>
		/// <see cref="DataTable"/> を <see cref="User"/> の <see cref="List{T}"/> に変換する。
		/// </summary>
		/// <param name="dt">変換元</param>
		/// <returns>変換したオブジェクト</returns>
		public static List<User> ConvertFrom(DataTable dt) {
			var ret = new List<User>();
			foreach (var dr in dt.AsEnumerable()) {
				var user = new User((int)dr["id"]);
				user.UserName = dr["user_name"].ToString()!;
				user.MailAddress = dr["mail_address"].ToString()!;
				user.Password = dr["password"].ToString()!;

				ret.Add(user);
			}

			return ret;
		}
		#endregion

		#region public methods
		/// <summary>
		/// ユーザをログイン状態に変更する。
		/// </summary>
		/// <returns>成否</returns>
		public bool Login() {
			var dao = new UserDAO();
			var ret = dao.UpdateOnlineFlag(this, true);
			if (ret) {
				this.IsOnline = true;
			}

			return ret;
		}
		#endregion

	}
}
