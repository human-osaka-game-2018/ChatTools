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

            DataContext = textAreaViewModel;
        }

        private void TB_textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            textAreaViewModel.KeyCheckCommand.Execute(null);
        }

        TextAreaViewModel textAreaViewModel = new TextAreaViewModel();
    }
}
