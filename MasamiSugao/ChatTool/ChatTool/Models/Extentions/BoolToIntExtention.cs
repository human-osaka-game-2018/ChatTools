namespace ChatTool.Models.Extentions {
	/// <summary>
	/// <see cref="bool"/> と数値の変換を行うための拡張クラス。
	/// </summary>
	public static class BoolToIntExtention {

		/// <summary>
		/// <see cref="bool"/> を <see cref="int"/> に変換する。
		/// </summary>
		/// <param name="target">変換対象</param>
		/// <returns>変換後の値</returns>
		public static int ToInt(this bool target) {
			return target ? 1 : 0;
		}

		/// <summary>
		/// <see cref="sbyte"/> を <see cref="bool"/> に変換する。
		/// </summary>
		/// <param name="target">変換対象</param>
		/// <returns>変換後の値</returns>
		public static bool ToBool(this sbyte target) {
			return (target != 0);
		}

	}
}

