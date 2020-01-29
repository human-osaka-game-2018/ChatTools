﻿using System;
using System.Windows;
using System.Windows.Controls;
using ChatTool.ViewModels;

namespace ChatTool.Views {
	/// <summary>
	/// LoginView.xaml の相互作用ロジック
	/// </summary>
	public partial class LoginView : Window {

		#region field members
		/// <summary>
		/// ViewModel.
		/// </summary>
		private LoginViewModel viewModel = new LoginViewModel();
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public LoginView() {
			InitializeComponent();

			this.DataContext = this.viewModel;
			this.viewModel.SuccessOfLoginEvent += this.OnSuccessOfLogin;
		}
		#endregion

		#region private methods
		/// <summary>
		/// PasswordBox変更イベントハンドラ。
		/// </summary>
		/// <param name="sender">イベントソース</param>
		/// <param name="e">イベントパラメータ</param>
		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
			this.viewModel.Password = ((PasswordBox)sender).Password;
		}

		/// <summary>
		/// ログイン成功イベントハンドラ。
		/// </summary>
		/// <param name="sender">イベントソース</param>
		/// <param name="e">イベントパラメータ</param>
		private void OnSuccessOfLogin(object? sender, EventArgs e) {
			var nextView = new MainView();
			nextView.Show();
			this.Close();
		}

		/// <summary>
		/// ログイン失敗イベントハンドラ。
		/// </summary>
		/// <param name="sender">イベントソース</param>
		/// <param name="e">イベントパラメータ</param>
		private void OnFailureOfLogin(object? sender, EventArgs e) {
			MessageBox.Show("メールアドレス又はパスワードが違います。", "ログイン", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		#endregion

	}
}

