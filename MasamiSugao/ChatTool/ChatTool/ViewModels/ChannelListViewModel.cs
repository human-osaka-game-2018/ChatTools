using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;

namespace ChatTool.ViewModels {
	/// <summary>
	/// チャンネル一覧画面のViewModel。
	/// </summary>
	public class ChannelListViewModel : BindableBase {

		#region field members
		/// <summary>選択中のチャンネル。</summary>
		private Channel? selectedChannel;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ChannelListViewModel() {
#if DEBUG
			// デザイナから実行時はテストデータ挿入
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				Enumerable.Range(1, 5).ToList().ForEach(i => this.Channels.Add(new Channel(i, $"チャンネル名{i}")));
				return;
			}
#endif

			var channelList = ChannelService.ListAvailableChannelsBy(LoginService.CurrentUser!);
			// 再描画が行われるように、ObservableCollectionの再生成ではなく、1件ずつ追加していく
			channelList.ForEach(x => this.Channels.Add(x));

			if (this.Channels.Count > 0) {
				this.SelectedChannel = this.Channels[0];
			}
		}
		#endregion

		#region properties
		/// <summary>
		/// チャンネルリスト。
		/// </summary>
		public ObservableCollection<Channel> Channels { get; } = new ObservableCollection<Channel>();

		/// <summary>
		/// 選択中のチャンネル。
		/// </summary>
		public Channel? SelectedChannel {
			get => this.selectedChannel;
			set {
				if (base.SetProperty(ref this.selectedChannel, value)) {
					ChannelService.CurrentChannel = value;
				}
			}
		}
		#endregion

	}
}

