namespace ChatTool.Models.DomainObjects {
	/// <summary>
	/// Entityの基底クラス。
	/// </summary>
	public class EntityBase : BindableBase {

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="id">ID</param>
		protected EntityBase(int id = -1) {
			this.Id = id;
		}
		#endregion

		#region properties
		/// <summary>
		/// メッセージID。
		/// </summary>
		public int Id { get; }
		#endregion

		#region operator overloads
		/// <summary>
		/// <see cref="EntityBase"/> 同士の等価比較を行う。
		/// </summary>
		/// <param name="obj1">比較対象1</param>
		/// <param name="obj2">比較対象2</param>
		/// <returns><see cref="Id"/> が同じ場合は <c>true</c></returns>
		public static bool operator==(EntityBase? obj1, EntityBase? obj2) {
			if (object.ReferenceEquals(obj1, null)) {
				return object.ReferenceEquals(obj1, obj2);
			}
			return obj1.Equals(obj2);
		}

		/// <summary>
		/// <see cref="EntityBase"/> 同士の不等価比較を行う。
		/// </summary>
		/// <param name="obj1">比較対象1</param>
		/// <param name="obj2">比較対象2</param>
		/// <returns><see cref="Id"/> が異なる場合は <c>true</c></returns>
		public static bool operator !=(EntityBase? obj1, EntityBase? obj2) {
			return !(obj1 == obj2);
		}
		#endregion

		#region public methods
		/// <summary>
		/// Returns a value indicating whether this instance is equal to a specified object.
		/// </summary>
		/// <param name="obj">An object to compare with this instance.</param>
		/// <returns><see cref="Id"/> が同じ場合は <c>true</c>.</returns>
		public override bool Equals(object? obj) {
			var entity = obj as EntityBase;
			return this.Id == entity?.Id;
		}

		/// <summary>
		/// Returns the hash code for this instance.
		/// </summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode() {
			return this.Id.GetHashCode();
		}
		#endregion

	}
}

