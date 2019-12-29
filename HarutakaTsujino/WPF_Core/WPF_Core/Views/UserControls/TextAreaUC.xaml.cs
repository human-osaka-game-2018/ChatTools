using System.Windows.Controls;
using WPF_Core.ViewModels.UserControls;

namespace WPF_Core.Views.UserControls
{
    /// <summary>
    /// TextAreaUC.xaml の相互作用ロジック
    /// </summary>
    public partial class TextAreaUC : UserControl
    {
        public TextAreaUC()
        {
            InitializeComponent();

            DataContext = new TextAreaViewModel();
        }
    }
}
