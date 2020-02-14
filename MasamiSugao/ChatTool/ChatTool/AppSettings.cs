using System.ComponentModel;
using System.Configuration;
using System.Windows;

namespace ChatTool {
	/// <summary>
	/// アプリケーション設定クラス。
	/// </summary>
	public class AppSettings {

		#region constants
		/// <summary>ユーザデフォルトアイコンのURI。</summary>
		public const string UserDefaultIconUri = ImagesDirectoryUri + "UserDefaultIcon.png";

		/// <summary>リアクションアイコンのURI。</summary>
		public const string ReactionIconUri = ImagesDirectoryUri + "ReactionIcons/";

		/// <summary>リアクション追加アイコンのURI。</summary>
		public const string AddReactionIconUri = ImagesDirectoryUri + "AddReactionIcon.png";

		/// <summary>ユーザアイコンフォルダのパス文字列取得用のApp.configのキー</summary>
		private const string KeyOfUserIconDirectoryPath = "userIconDirectoryPath";

		/// <summary>画像フォルダのURI。</summary>
		private const string ImagesDirectoryUri = "pack://application:,,,/Images/";

		#endregion

		#region constructors
		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static AppSettings() {
#if DEBUG
			// デザイナから実行時はapp.configが読めないのでブランク設定
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				UserIconDirectoryPath = "";
				return;
			}
#endif
			UserIconDirectoryPath = ConfigurationManager.AppSettings[KeyOfUserIconDirectoryPath];
		}
		#endregion

		#region properties
		/// <summary>
		/// ユーザアイコンディレクトリパス。
		/// </summary>
		public static string UserIconDirectoryPath { get; }
		#endregion

	}
}

