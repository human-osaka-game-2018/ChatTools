using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace ChatTool {
	/// <summary>
	/// Interaction logic for App.xaml.
	/// </summary>
	public partial class App : Application {

		#region constants
		/// <summary>アプリケーション名。</summary>
		public const string AppName = "ChatTool";
		#endregion

		#region constructors
		public App() {
			// 未処理例外の処理
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		}
		#endregion

		#region properties
		/// <summary>
		/// 例外処理中フラグ。
		/// </summary>
		public static bool IsHandlingException { get; set; }
		#endregion

		#region private methods
		/// <summary>
		/// 最終的に処理されなかった未処理例外を処理します。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
			App.IsHandlingException = true;
			var exception = e.ExceptionObject as Exception;
			this.ShowExceptionMessage(exception);
		}

		/// <summary>
		/// 例外を通知するメッセージボックスを表示する。
		/// </summary>
		/// <param name="e">例外オブジェクト</param>
		private void ShowExceptionMessage(Exception? e) {
			var message = new StringBuilder();
			this.AppendExceptionMessages(message, e);

			Debug.WriteLine(message.ToString());
			try {
				MessageBox.Show(message.ToString(), AppName, MessageBoxButton.OK, MessageBoxImage.Error);
			} catch (Exception ex) {
				this.ShowExceptionMessage(ex);
			}
		}

		/// <summary>
		/// 例外を通知するメッセージの文面を構築する。
		/// </summary>
		/// <param name="message">構築したメッセージを入れる<see cref="StringBuilder"/></param>
		/// <param name="e">例外オブジェクト</param>
		private void AppendExceptionMessages(StringBuilder message, Exception? e) {
			message.AppendLine($"未処理例外発生。");
			message.AppendLine();
			if (e != null) {
				message.AppendLine($"{e.Message} @ {e.TargetSite?.Name}");
				message.AppendLine();
				message.AppendLine(e.StackTrace);
				message.AppendLine();
				message.AppendLine();

				if (e.InnerException != null) {
					this.AppendExceptionMessages(message, e.InnerException);
				}
			}
		}
		#endregion

	}
}

