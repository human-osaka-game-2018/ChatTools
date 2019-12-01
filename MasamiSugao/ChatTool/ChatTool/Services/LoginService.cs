using System.Collections.Generic;
using System.Linq;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Services {
	/// <summary>
	/// ログイン関連の処理を行うサービスクラス。
	/// </summary>
	public static class LoginService {

		#region properties
		/// <summary>
		/// 現在ログイン中のユーザ
		/// </summary>
		public static User? CurrentUser { get; private set; }
		#endregion

		#region public methods
		/// <summary>
		/// ログインする。
		/// </summary>
		/// <param name="mailAddress">メールアドレス</param>
		/// <param name="password">パスワード</param>
		/// <returns>ログインに成功したかどうか</returns>
		public static bool LogIn(string mailAddress, string password) {
			bool ret;
			var userDAO = new UserDAO();

			// メールアドレスが一致するユーザ取得
			var users = userDAO.Select(mailAddress);
			if (users.Count == 0) return false;

			// 認証
			var user = Authenticate(users, mailAddress, password);
			ret = (user != null);

			// オンラインフラグ更新
			ret = ret && user!.Login();

			if (ret) {
				// 認証成功した場合はログイン中ユーザを保持
				CurrentUser = user;
			}

			return ret;
		}
		#endregion

		#region private methods
		/// <summary>
		/// ユーザ認証を行う。
		/// </summary>
		/// <param name="users">認証対象のユーザ一覧</param>
		/// <param name="mailAddress">メールアドレス</param>
		/// <param name="password">パスワード</param>
		/// <returns>認証に成功したユーザ情報。に印象失敗した場合は<c>null</c></returns>
		private static User? Authenticate(List<User> users, string mailAddress, string password) {
			var user = users.FirstOrDefault(x => x.MailAddress == mailAddress && x.Password == password);

			return user;
		}
		#endregion

	}
}
