using System.Collections.ObjectModel;
using ChatTool.Models.DomainObjects;
using ChatTool.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// チャンネル一覧画面のViewModel.
	/// </summary>
	public class ChannelListViewModel {

		/// <summary>
		/// チャンネル一覧データ。
		/// </summary>
		private ChannelListService service = new ChannelListService();

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ChannelListViewModel() {
			// デザイナでのエラーを防ぐために、初期表示処理はLoadedイベントで行うようにする
			this.LoadedCommand = new DelegateCommand(this.OnLoaded);
		}

		/// <summary>
		/// チャンネルリスト。
		/// </summary>
		public ObservableCollection<Channel> Channels { get; } = new ObservableCollection<Channel>();

		/// <summary>
		/// 画面ロードコマンド。
		/// </summary>
		public DelegateCommand LoadedCommand { get; }

		/// <summary>
		/// 画面ロードイベントハンドラ。
		/// </summary>
		private void OnLoaded() {
			var channelList = service.CreateList();
			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			channelList.ForEach(x => this.Channels.Add(x));
		}

	}
}
