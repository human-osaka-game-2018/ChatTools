using ChatTool.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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

namespace ChatTool.Views.Main
{
    /// <summary>
    /// MessageLogView.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageListView : UserControl
    {
        public MessageListView()
        {
            InitializeComponent();
            DataContext = messageListViewModel;
        }                

        private MessageListViewModel messageListViewModel = new MessageListViewModel();

        private void ListMessage_TargetUpdated(object sender, DataTransferEventArgs e)
        {            
            var Items = this.ListMessage.Items;

            if (Items.Count <= 0) return;

            ListMessage.ScrollIntoView(Items[Items.Count - 1]);
        }
    }
}
