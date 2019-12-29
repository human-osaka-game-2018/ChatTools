using System;
using System.Windows;
using WPF_Core.Models.Services;

namespace WPF_Core.Views
{
    /// <summary>
    /// EditorView.xaml の相互作用ロジック
    /// </summary>
    public partial class EditorView : Window
    {
        public EditorView()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LogInService.LogOut();
        }

        private void ChannelsDisplayUC_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
