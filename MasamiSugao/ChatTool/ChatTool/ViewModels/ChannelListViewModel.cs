using System.Collections.ObjectModel;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// チャンネル一覧画面のViewModel.
	/// </summary>
	public class ChannelListViewModel : BindableBase {

		#region field members
		/// <summary>
		/// 選択中のチャンネル。
		/// </summary>
		private Channel? selectedChannel;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ChannelListViewModel() {
			// デザイナでのエラーを防ぐために、初期表示処理はLoadedイベントで行うようにする
			this.LoadedCommand = new DelegateCommand(this.OnLoaded);
		}
		#endregion

		#region properties
		/// <summary>
		/// チャンネルリスト。
		/// </summary>
		public ObservableCollection<Channel> Channels { get; } = new ObservableCollection<Channel>();

		public Channel? SelectedChannel {
			get => this.selectedChannel;
			set {
				base.SetProperty(ref this.selectedChannel, value);
				ChannelService.CurrentChannel = value;
			}
		}
		/// <summary>
		/// 画面ロードコマンド。
		/// </summary>
		public DelegateCommand LoadedCommand { get; }
		#endregion

		#region private methods
		/// <summary>
		/// 画面ロードイベントハンドラ。
		/// </summary>
		private void OnLoaded() {
			var channelList = ChannelService.ListAvailableChannelsBy(LoginService.CurrentUser!);
			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			channelList.ForEach(x => this.Channels.Add(x));
		}
		#endregion

	}
}
