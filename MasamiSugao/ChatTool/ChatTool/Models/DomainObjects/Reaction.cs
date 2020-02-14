using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// リアクション。
	/// </summary>
	public class Reaction : EntityBase {

		#region constants/readonly
		/// <summary>リアクションマスタ一覧。</summary>
		private static readonly Dictionary<int, Reaction> all = new Dictionary<int, Reaction>();
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="raectionName">リアクション名</param>
		public Reaction(int id, string raectionName) : base(id) {
			this.ReactionName = raectionName;

			var iconUri = new Uri($"{AppSettings.ReactionIconUri}{this.ReactionName}.png");
			this.Icon = new BitmapImage(iconUri);

			all.Add(this.Id, this);
		}
		#endregion

		#region properties
		/// <summary>
		/// リアクションマスタ一覧。
		/// </summary>
		public static ReadOnlyDictionary<int, Reaction> All { get; } = new ReadOnlyDictionary<int, Reaction>(all);

		/// <summary>
		/// リアクション名。
		/// </summary>
		public string ReactionName { get; }

		/// <summary>
		/// リアクションアイコン。
		/// </summary>
		public BitmapSource Icon { get; }
		#endregion

		#region public static methods
		/// <summary>
		/// <see cref="DataTable"/> を <see cref="Reaction"/> の <see cref="List{T}"/> に変換する。
		/// </summary>
		/// <param name="dt">変換元</param>
		/// <returns>変換したオブジェクト</returns>
		public static List<Reaction> ConvertFrom(DataTable dt) {
			var reactions = dt.AsEnumerable().Select(dr => new Reaction(dr.Field<int>("id"), dr.Field<string>("reaction_name")));

			return reactions.ToList();
		}

		/// <summary>
		/// 指定したIDのリアクションを取得する。
		/// </summary>
		/// <param name="id">リアクションID</param>
		/// <returns>リアクションマスタデータ</returns>
		public static Reaction Get(int id) {
			return all[id];
		}
		#endregion

	}
}

