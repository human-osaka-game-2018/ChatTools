using System;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// ログイン画面のViewModel。
	/// </summary>
	public class LoginViewModel {

		#region constants
		/// <summary>画面名。</summary>
		private const string ViewName = "ログイン";
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public LoginViewModel() {
			this.BtnLoginClickCommand = new DelegateCommand(this.OnBtnLoginClicked);
		}
		#endregion

		#region events
		/// <summary>
		/// ログイン成功イベント。
		/// </summary>
		public event EventHandler? SuccessOfLoginEvent;

		/// <summary>
		/// ログイン失敗イベント。
		/// </summary>
		public event EventHandler? FailureOfLoginEvent;
		#endregion

		#region parameters
		/// <summary>
		/// タイトルバー表示文字列。
		/// </summary>
		public string Title { get; } = $"{App.AppName}-{ViewName}";

		/// <summary>
		/// メールアドレス入力内容。
		/// </summary>
		public string MailAddress { get; set; } = string.Empty;

		/// <summary>
		/// パスワード。
		/// </summary>
		public string Password { get; set; } = string.Empty;

		/// <summary>
		/// ログインボタン押下コマンド。
		/// </summary>
		public DelegateCommand BtnLoginClickCommand { get; }
		#endregion

		#region private methods
		/// <summary>
		/// ログインボタン押下時の処理。
		/// </summary>
		private void OnBtnLoginClicked() {
			if (LoginService.LogIn(this.MailAddress, this.Password)) {
				this.SuccessOfLoginEvent?.Invoke(this, EventArgs.Empty);
			} else {
				this.FailureOfLoginEvent?.Invoke(this, EventArgs.Empty);
			}
		}
		#endregion

	}
}

