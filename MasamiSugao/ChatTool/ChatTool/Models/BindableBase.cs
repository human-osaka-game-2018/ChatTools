using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChatTool.Models {
	/// <summary>
	/// モデルを簡略化するための <see cref="INotifyPropertyChanged"/> の実装。
	/// </summary>
	public class BindableBase : INotifyPropertyChanged {

		/// <summary>
		/// プロパティの変更を通知するためのマルチキャスト イベント。
		/// </summary>
		public event PropertyChangedEventHandler? PropertyChanged;

		/// <summary>
		/// プロパティが既に目的の値と一致しているかどうかを確認します。
		/// 必要な場合のみプロパティを設定し、リスナーに通知します。
		/// </summary>
		/// <typeparam name="T">プロパティの型。</typeparam>
		/// <param name="storage">get アクセス操作子と set アクセス操作子両方を使用したプロパティへの参照。</param>
		/// <param name="value">プロパティに必要な値。</param>
		/// <param name="propertyName">
		/// <para>リスナーに通知するために使用するプロパティの名前。</para>
		/// <para>この値は省略可能で<see cref="CallerMemberNameAttribute"/>をサポートするコンパイラから呼び出す場合に自動的に指定できます。</para>
		/// </param>
		/// <returns>値が変更された場合は<c>true</c>、既存の値が目的の値に一致した場合は<c>false</c>です。</returns>
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String? propertyName = null) {
			if (object.Equals(storage, value)) return false;

			storage = value;
			this.OnPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// プロパティ値が変更されたことをリスナーに通知します。
		/// </summary>
		/// <param name="propertyName">
		/// <para>リスナーに通知するために使用するプロパティの名前。</para>
		/// <para>この値は省略可能で、<see cref="CallerMemberNameAttribute"/> をサポートするコンパイラから呼び出す場合に自動的に指定できます。</para>
		/// </param>
		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}

