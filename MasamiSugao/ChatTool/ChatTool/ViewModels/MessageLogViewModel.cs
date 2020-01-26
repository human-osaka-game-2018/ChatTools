using System.Collections.ObjectModel;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// メッセージ画面のViewModel.
	/// </summary>
	public class MessageLogViewModel : BindableBase {

		#region field members
		/// <summary>
		/// 選択中のメッセージ。
		/// </summary>
		private Message? selectedMessage;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessageLogViewModel() {
			// デザイナでのエラーを防ぐために、初期表示処理はLoadedイベントで行うようにする
//			this.LoadedCommand = new DelegateCommand(this.OnLoaded);

			ChannelService.OnChannelChanged += (_, channel) => this.OnChannelChanged(channel);

			this.Messages.Add(new Message(0) { Text = "message", User = new User(0) { UserName = "user" } });
		}
		#endregion

		#region properties
		/// <summary>
		/// メッセージリスト。
		/// </summary>
		public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

		public Message? SelectedMessage {
			get => this.selectedMessage;
			set {
				base.SetProperty(ref this.selectedMessage, value);
			}
		}
		/// <summary>
		/// 画面ロードコマンド。
		/// </summary>
		//public DelegateCommand LoadedCommand { get; }
		#endregion

		#region private methods
		/// <summary>
		/// 画面ロードイベントハンドラ。
		/// </summary>
		private void OnChannelChanged(Channel channel) {
			//var channelList = ChannelService.ListAvailableChannelsBy(LoginService.CurrentUser!);
			//// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			//channelList.ForEach(x => this.Channels.Add(x));
			var messages = MessageService.ListMessagesBy(channel);
			this.Messages.Clear();
			messages.ForEach(x => this.Messages.Add(x));
		}
		#endregion

	}
}
