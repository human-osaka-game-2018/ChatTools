using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// 入力画面のViewModel.
	/// </summary>
	public class InputAreaViewModel : BindableBase {

		#region field members
		/// <summary>サービスクラス。</summary>
		private MessageService service = new MessageService();

		/// <summary>入力文字列。</summary>
		private string inputMessage = string.Empty;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public InputAreaViewModel() {
			this.BtnSendClickedCommand = new DelegateCommand(() => this.OnBtnSendClicked(), () => this.CanSend);

			MessageService.OnMessagePosted += (_, __) => this.OnMessagePosted();
		}
		#endregion

		#region properties
		/// <summary>
		/// 入力文字列。
		/// </summary>
		public string InputMessage {
			get => this.inputMessage;
			set {
				base.SetProperty(ref this.inputMessage, value);
				this.BtnSendClickedCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// 送信ボタン押下コマンド。
		/// </summary>
		public DelegateCommand BtnSendClickedCommand { get; }

		/// <summary>
		/// 送信可能かどうかを示すフラグ。
		/// </summary>
		public bool CanSend => ChannelService.CurrentChannel == null || this.inputMessage.Length > 0;
		#endregion

		#region private methods
		/// <summary>
		/// 送信ボタン押下時の処理。
		/// </summary>
		private void OnBtnSendClicked() {
			var message = new Message(ChannelService.CurrentChannel!, LoginService.CurrentUser!, this.InputMessage);
			this.service.Post(message);
		}

		/// <summary>
		/// メッセージ送信完了時の処理。
		/// </summary>
		private void OnMessagePosted() {
			this.InputMessage = string.Empty;
		}
		#endregion

	}
}
