using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using ChatTool.ViewModels.Main;
using System.Diagnostics;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;

namespace ChatTool.Views.Main
{
    /// <summary>
    /// MessageLogView.xaml の相互作用ロジック
    /// </summary>
    public partial class MessageLogView : UserControl
    {
        public MessageLogView()
        {
            InitializeComponent();
            ReadMessageService.RefleshMessageLog += () => { ScrollToBottom(); };
            var viewModel = new MessageViewModel();
            this.DataContext = viewModel;
            ReplyMessageService.EraseSelectingReply += (_) => { ClearSelectedItem(); };
        }

        private void ScrollToBottom()
        {
            var peer = UIElementAutomationPeer.CreatePeerForElement(this.listBox);
            // GetPatternでIScrollProviderを取得
            var scrollProvider = peer.GetPattern(PatternInterface.Scroll) as IScrollProvider;
            if (scrollProvider != null && scrollProvider.VerticallyScrollable)
            {
                scrollProvider?.SetScrollPercent(scrollProvider.HorizontalScrollPercent, 100);
                scrollProvider?.Scroll(ScrollAmount.NoAmount, ScrollAmount.LargeIncrement);
            }
        }

        private void ClearSelectedItem()
        {
            listBox.SelectedIndex = -1;
            listBox.Focus();
        }

        private void ChildSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var sendObj = sender as ListBox;
            var message = sendObj?.SelectedItem as Message;
            if (null == message || null == sendObj) return;
            Debug.Write("ID" + message?.Id.ToString() + "で,親が" + message?.ParentId.ToString() + "\n");
            foreach (Message? ListItem in listBox.ItemsSource)
            {
                if (ListItem?.Id == message?.ParentId)
                {
                    listBox.SelectedItem = ListItem;
                }
            }
            sendObj.SelectedIndex = -1;
        }
    }
}
