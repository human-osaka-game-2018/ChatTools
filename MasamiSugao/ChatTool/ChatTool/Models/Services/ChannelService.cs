using System;
using System.Collections.Generic;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Models.Services {
	/// <summary>
	/// チャンネルに関するサービスクラス。
	/// </summary>
	public static class ChannelService {

		#region field members
		/// <summary>現在選択中のチャンネル。</summary>
		private static Channel? currentChannel;
		#endregion

		#region events
		/// <summary>
		/// 選択チャンネル変更イベント。
		/// </summary>
		public static event EventHandler<Channel>? OnChannelChanged;
		#endregion

		#region properties
		/// <summary>
		/// 現在選択中のチャンネル。
		/// </summary>
		public static Channel? CurrentChannel {
			get => currentChannel;
			set {
				if (currentChannel == value) return;

				currentChannel = value;
				if (value != null) {
					OnChannelChanged?.Invoke(null, value);
				}
			}
		}
		#endregion

		#region public static methods
		/// <summary>
		/// チャンネル一覧データを生成する。
		/// </summary>
		/// <returns>一覧データ</returns>
		public static List<Channel> ListAvailableChannelsBy(User user) {
			var channelDAO = new ChannelDAO();
			var dt = channelDAO.SelectBelongingChannels(user);
			return Channel.ConvertFrom(dt);
		}
		#endregion

	}
}

