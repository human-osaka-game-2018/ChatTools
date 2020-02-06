﻿using ChatTool.Infrastructure.Database;
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
	public class User {

		#region constants/constants
		/// <summary>アイコンフォルダパス。</summary>
		private const string IconFolderName = "UserIcons";

		/// <summary>デフォルトアイコン。</summary>
		private static readonly BitmapSource defaultIcon;
		#endregion

		#region constructors
		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static User() {
#if DEBUG
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				defaultIcon = new BitmapImage();
			} else {
#endif
				var uri = new Uri(AppSettings.UserDefaultIconUri);
				defaultIcon = new BitmapImage(uri);
#if DEBUG
			}
#endif
		}

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ユーザID</param>
		public User(int id) {
			this.Id = id;
		}
		#endregion

		#region properties
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
		public int IconId { get; set; }

		/// <summary>
		/// アイコン画像パス。
		/// </summary>
		public BitmapSource IconPath {
			get {
				var folderPath = Path.GetFullPath(Path.Combine(AppSettings.ResourceDirectoryPath, IconFolderName, this.IconId.ToString("00000")));
				if (!Directory.Exists(folderPath)) {
					return defaultIcon;
				}

				var uri = new Uri(Directory.GetFiles(folderPath, "*.*")[0]);
				var bmp = new BitmapImage(uri);
				return bmp;
			}
		}

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
			var users = dt.AsEnumerable().Select(dr =>
				 new User(dr.Field<int>("id")) {
					UserName = dr.Field<string>("user_name"),
					MailAddress = dr.Field<string>("mail_address"),
					IconId = dr.Field<int>("icon_id"),
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

