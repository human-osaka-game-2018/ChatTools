using System;
using System.Windows.Input;

namespace ChatTool.ViewModels {
	/// <summary>
	/// コマンド委譲クラス。
	/// </summary>
	public class DelegateCommand : ICommand {

		#region field members
		/// <summary>
		/// コマンド実行時の処理。
		/// </summary>
		private Action? execute;

		/// <summary>
		/// コマンドが実行可否の判断方法。
		/// </summary>
		private Func<bool> canExecute;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">コマンド実行時の処理</param>
		/// <param name="canExecute">コマンド実行可否の判断方法</param>
		public DelegateCommand(Action? execute, Func<bool>? canExecute = null) {
			this.execute = execute;
			this.canExecute = canExecute ?? new Func<bool>(() => true);
		}
		#endregion

		#region events
		/// <summary>
		/// コマンド実行可否変更イベント。
		/// </summary>
		public event EventHandler? CanExecuteChanged;
		#endregion

		#region public methods
		/// <summary>
		/// コマンドが実行可能かどうか。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		/// <returns>コマンド実行可否</returns>
		bool ICommand.CanExecute(object? parameter) {
			return this.CanExecute();
		}

		/// <summary>
		/// コマンドを実行する。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		void ICommand.Execute(object? parameter) {
			this.Execute();
		}

		/// <summary>
		/// コマンドが実行可能かどうか。
		/// </summary>
		/// <returns>コマンド実行可否</returns>
		public bool CanExecute() {
			return this.canExecute();
		}

		/// <summary>
		/// コマンドを実行する。
		/// </summary>
		public void Execute() {
			this.execute?.Invoke();
		}

		/// <summary>
		/// コマンド実行可否変更イベントを発火する。
		/// </summary>
		public void RaiseCanExecuteChanged() {
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

	}
}
