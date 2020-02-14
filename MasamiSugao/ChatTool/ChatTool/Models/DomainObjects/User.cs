using ChatTool.Infrastructure.Database;
using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// ユーザ情報クラス。
	/// </summary>
	public class User : EntityBase {

		#region constants/readonly
		/// <summary>デフォルトアイコン。</summary>
		private static readonly BitmapSource defaultIcon;
		#endregion

		#region constructors
		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static User() {
#if DEBUG
			// デザイナから実行時はファイルアクセス処理を呼び出さないようにする
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				defaultIcon = new BitmapImage();
				return;
			}
#endif
			var uri = new Uri(AppSettings.UserDefaultIconUri);
			defaultIcon = new BitmapImage(uri);
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ユーザID</param>
		/// <param name="UserName">ユーザ名</param>
		/// <param name="iconId">アイコンID</param>
		public User(int id, int iconId, string UserName) : base(id) {
			this.IconId = iconId;
			this.UserName = UserName;
			this.Icon = this.CreateIconImage();
		}
		#endregion

		#region properties
		/// <summary>
		/// ユーザ名。
		/// </summary>
		public string UserName { get; }

		/// <summary>
		/// メールアドレス。
		/// </summary>
		public string MailAddress { get; private set; } = string.Empty;

		/// <summary>
		/// パスワード。
		/// </summary>
		public string Password { get; private set; } = string.Empty;

		/// <summary>
		/// アイコンID。
		/// </summary>
		public int IconId { get; }

		/// <summary>
		/// アイコン画像。
		/// </summary>
		public BitmapSource Icon { get; }

		/// <summary>
		/// オンラインフラグ。
		/// </summary>
		public bool IsOnline { get; private set; } = false;

		/// <summary>
		/// ログインユーザかどうか。
		/// </summary>
		public bool IsCurrentUser => (this == LoginService.CurrentUser);
		#endregion

		#region public static methods
		/// <summary>
		/// <see cref="DataTable"/> を <see cref="User"/> の <see cref="List{T}"/> に変換する。
		/// </summary>
		/// <param name="dt">変換元</param>
		/// <returns>変換したオブジェクト</returns>
		public static List<User> ConvertFrom(DataTable dt) {
			var users = dt.AsEnumerable().Select(dr =>
				 new User(dr.Field<int>("id"), dr.Field<int>("icon_id"), dr.Field<string>("user_name")) {
					MailAddress = dr.Field<string>("mail_address"),
					Password = dr.Field<string?>("password") ?? string.Empty
				});

			return users.ToList();
		}
		#endregion

		#region public methods
		/// <summary>
		/// ユーザをログイン状態に変更する。
		/// </summary>
		/// <returns>成否</returns>
		public bool LogIn() {
			var dao = new UserDAO();
			var ret = dao.UpdateOnlineFlag(this, true);
			if (ret) {
				this.IsOnline = true;
			}

			return ret;
		}

		/// <summary>
		/// ユーザをログアウト状態に変更する。
		/// </summary>
		/// <returns>成否</returns>
		public bool LogOut() {
			var dao = new UserDAO();
			var ret = dao.UpdateOnlineFlag(this, false);
			if (ret) {
				this.IsOnline = false;
			}

			return ret;
		}
		#endregion

		#region private methods
		/// <summary>
		/// アイコン画像を生成する。
		/// </summary>
		/// <returns>生成した画像</returns>
		private BitmapSource CreateIconImage() {
			var folderPath = Path.GetFullPath(Path.Combine(AppSettings.UserIconDirectoryPath, this.IconId.ToString("D5")));
			if (!Directory.Exists(folderPath)) {
				return defaultIcon;
			}

			var uri = new Uri(Directory.GetFiles(folderPath, "*.*")[0]);
			var bmp = new BitmapImage(uri);
			return bmp;
		}
		#endregion

	}
}

