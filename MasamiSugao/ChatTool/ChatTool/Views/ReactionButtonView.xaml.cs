using System.Windows;
using System.Windows.Controls;
using ChatTool.Models.DomainObjects;

namespace ChatTool.Views {
	/// <summary>
	/// ReactionButtonView.xaml の相互作用ロジック。
	/// </summary>
	public partial class ReactionButtonView : UserControl {

		/// <summary>
		/// ReactionLog依存プロパティ。
		/// </summary>
		public static readonly DependencyProperty ReactionLogProperty = DependencyProperty.Register(
																			"ReactionLog",
																			typeof(ReactionLogCollection),
																			typeof(ReactionButtonView),
																			new FrameworkPropertyMetadata(null));

		/// <summary>
		/// コンストラクタ。
		/// </summary>
		public ReactionButtonView() {
			InitializeComponent();
		}

		/// <summary>
		/// リアクション情報。
		/// </summary>
		public ReactionLogCollection ReactionLog {
			get => (ReactionLogCollection)base.GetValue(ReactionLogProperty);
			set => base.SetValue(ReactionLogProperty, value);
		}

	}
}

