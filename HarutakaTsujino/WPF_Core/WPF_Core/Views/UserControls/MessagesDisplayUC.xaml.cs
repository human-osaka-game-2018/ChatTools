using System.Windows.Controls;
using WPF_Core.ViewModels.UserControls;

namespace WPF_Core.Views.UserControls
{
    /// <summary>
    /// MessagesDisplayUC.xaml の相互作用ロジック
    /// </summary>
    public partial class MessagesDisplayUC : UserControl
    {
        public MessagesDisplayUC()
        {
            InitializeComponent();
            DataContext = messagesDisplayViewModel;
        }

        private readonly MessagesDisplayViewModel messagesDisplayViewModel = new MessagesDisplayViewModel();
    }
}
