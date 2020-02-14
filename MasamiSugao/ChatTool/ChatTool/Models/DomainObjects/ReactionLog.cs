namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// ReactionLog情報を格納するクラス。
	/// </summary>
	public class ReactionLog : EntityBase {

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ReactionLogID</param>
		/// <param name="user">ユーザ</param>
		/// <param name="reaction">リアクション</param>
		public ReactionLog(int id, User user, Reaction reaction) : base(id) {
			this.User = user;
			this.Reaction = reaction;
		}

		/// <summary>
		/// リアクションしたユーザ。
		/// </summary>
		public User User { get; }

		/// <summary>
		/// リアクション。
		/// </summary>
		public Reaction Reaction { get; }

	}
}

