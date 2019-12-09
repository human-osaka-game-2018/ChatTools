using System.Collections.Generic;
using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Services {
	/// <summary>
	/// チャンネル一覧画面のサービスクラス。
	/// </summary>
	public class ChannelListService {

		/// <summary>
		/// 一覧データを生成する。
		/// </summary>
		/// <returns>一覧データ</returns>
		/// <remarks>取得対象はログイン中のユーザが所属するチャンネル</remarks>
		public List<Channel> CreateList() {
			var channelDAO = new ChannelDAO();
			return channelDAO.SelectBelongingChannels(LoginService.CurrentUser!);
		}

	}
}
