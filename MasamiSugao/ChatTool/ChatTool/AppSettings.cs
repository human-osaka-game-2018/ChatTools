using System.Configuration;

namespace ChatTool {
	/// <summary>
	/// アプリケーション設定クラス。
	/// </summary>
	public class AppSettings {

		/// <summary>
		/// ユーザデフォルトアイコンのURI。
		/// </summary>
		public const string UserDefaultIconUri = "pack://application:,,,/Images/UserDefaultIcon.png";

		/// <summary>
		/// 静的コンストラクタ。
		/// </summary>
		static AppSettings() {
			ResourceDirectoryPath = ConfigurationManager.AppSettings["resourceDirectoryPath"];
		}

		/// <summary>
		/// Resourcesディレクトリパス。
		/// </summary>
		public static string ResourceDirectoryPath { get; }

	}
}
