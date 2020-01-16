using System;
using System.Windows;
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

            messagesDisplayViewModel.OnMessagePostedAsObservable
                .Subscribe(_ => ScrollToLatest());

            Loaded += new RoutedEventHandler((s, e) => ScrollToLatest());
        }

        public void ScrollToLatest()
        {
            var messages = LB_messages.Items;

            LB_messages.ScrollIntoView(messages[messages.Count - 1]);
        }

        private readonly MessagesDisplayViewModel messagesDisplayViewModel = new MessagesDisplayViewModel();
    }
}
