using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
