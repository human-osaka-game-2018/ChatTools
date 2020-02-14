using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using ChatTool.Models;
using ChatTool.Models.DomainObjects;

namespace ChatTool.ViewModels {
	/// <summary>
	/// メッセージ表示パネルのViewModel。
	/// </summary>
	public class MessagePanelViewModel : BindableBase {

		#region field members
		/// <summary>表示するメッセージ。</summary>
		private Message? message;

		/// <summary>リアクションリストのポップアップが表示中かどうか。</summary>
		public bool isReactionListPopupOpen;
		#endregion

		#region constructors
		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public MessagePanelViewModel() {
			this.ReactionTypeButtonClickCommand = new DelegateCommand<Reaction>(x => this.OnReactionTypeButtonClicked(x));
			this.BtnAddReactionClickCommand = new DelegateCommand(() => this.OnBtnAddReactionClicked());
			this.AllReaction = new ObservableCollection<Reaction>(Reaction.All.Values);
			this.AddReactionIconPath = AppSettings.AddReactionIconUri;

#if DEBUG
			// デザイナから実行時はテストデータ挿入
			if ((bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue) {
				this.message = new Message(new Channel(0, "チャンネル名"), new User(0, 0, "ユーザ名"), "メッセージ本文");
				return;
			}
#endif
		}
		#endregion

		#region properties
		/// <summary>
		///リアクション追加アイコンのパス。
		/// </summary>
		public string AddReactionIconPath { get; }

		/// <summary>
		/// 表示するメッセージ。
		/// </summary>
		public Message? Message {
			get => this.message;
			set {
				if (value == null) return;

				// 複数スレッドからの要素変更を許可する
				BindingOperations.EnableCollectionSynchronization(value.ReactionLogs, new object());
				base.SetProperty(ref this.message, value);
			}
		}

		/// <summary>
		/// リアクションリスト表示対象。
		/// </summary>
		public ObservableCollection<Reaction> AllReaction { get; }

		/// <summary>
		/// リアクションリストのポップアップが表示中かどうか。
		/// </summary>
		public bool IsReactionListPopupOpen {
			get => this.isReactionListPopupOpen;
			set => base.SetProperty(ref this.isReactionListPopupOpen, value);
		}

		/// <summary>
		/// リアクション追加ボタンクリックコマンド。
		/// </summary>
		public DelegateCommand BtnAddReactionClickCommand { get; }

		/// <summary>
		/// リアクション種類ボタンクリックコマンド。
		/// </summary>
		public DelegateCommand<Reaction> ReactionTypeButtonClickCommand { get; }
		#endregion

		#region private methods;
		/// <summary>
		/// リアクション追加ボタン押下時の処理。
		/// </summary>
		private void OnBtnAddReactionClicked() {
			this.IsReactionListPopupOpen = true;
		}

		/// <summary>
		/// リアクション種類ボタン押下時の処理。
		/// </summary>
		/// <param name="reaction">リアクションの種類</param>
		private void OnReactionTypeButtonClicked(Reaction reaction) {
			this.IsReactionListPopupOpen = false;
			this.message!.AddReaction(reaction);
		}
		#endregion

	}
}

