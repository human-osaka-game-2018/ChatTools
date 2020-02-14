using System;
using System.Windows.Input;

namespace ChatTool.ViewModels {
	/// <summary>
	/// コマンド委譲クラスの既定クラス。
	/// </summary>
	public abstract class DelegateCommandBase : ICommand {

		#region events
		/// <summary>
		/// コマンド実行可否変更イベント。
		/// </summary>
		public event EventHandler? CanExecuteChanged;
		#endregion

		#region abstract methods
		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// If the command does not require data to be passed, this object can be set to null.
		/// </param>
		/// <returns>true if this command can be executed; otherwise, false.</returns>
		public abstract bool CanExecute(object parameter);

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// If the command does not require data to be passed, this object can be set to null.
		/// </param>
		public abstract void Execute(object parameter);
		#endregion

		#region public methods
		/// <summary>
		/// コマンド実行可否変更イベントを発火する。
		/// </summary>
		public void RaiseCanExecuteChanged() {
			this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
		}
		#endregion

	}

	/// <summary>
	/// コマンド委譲クラス。
	/// </summary>
	public class DelegateCommand : DelegateCommandBase {

		#region constants/readonly
		/// <summary>コマンド実行時の処理。</summary>
		private readonly Action? execute;

		/// <summary>コマンドが実行可否の判断方法。</summary>
		private readonly Func<bool> canExecute;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		/// <param name="execute">コマンド実行時の処理</param>
		/// <param name="canExecute">コマンド実行可否の判断方法</param>
		public DelegateCommand(Action? execute = null, Func<bool>? canExecute = null) {
			this.execute = execute;
			this.canExecute = canExecute ?? new Func<bool>(() => true);
		}
		#endregion

		#region public methods
		/// <summary>
		/// コマンドが実行可能かどうか。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		/// <returns>コマンド実行可否</returns>
		public override bool CanExecute(object? parameter) {
			return this.CanExecute();
		}

		/// <summary>
		/// コマンドを実行する。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		public override void Execute(object? parameter) {
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
		#endregion

	}

	/// <summary>
	/// コマンド委譲クラスのGenerics版。
	/// </summary>
	public class DelegateCommand<T> : DelegateCommandBase {

		#region constants/readonly
		/// <summary>コマンド実行時の処理。</summary>
		private readonly Action<T>? execute;

		/// <summary>コマンドが実行可否の判断方法。</summary>
		private readonly Func<T, bool> canExecute;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="execute">コマンド実行時の処理</param>
		/// <param name="canExecute">コマンド実行可否の判断方法</param>
		public DelegateCommand(Action<T>? execute = null, Func<T, bool>? canExecute = null) {
			this.execute = execute;
			this.canExecute = canExecute ?? new Func<T, bool>(_ => true);
		}
		#endregion

		#region public methods
		/// <summary>
		/// コマンドが実行可能かどうか。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		/// <returns>コマンド実行可否</returns>
		public override bool CanExecute(object parameter) {
			return this.canExecute((T)parameter);
		}

		/// <summary>
		/// コマンドを実行する。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
		public override void Execute(object parameter) {
			this.execute?.Invoke((T)parameter);
		}
		#endregion

	}
}

