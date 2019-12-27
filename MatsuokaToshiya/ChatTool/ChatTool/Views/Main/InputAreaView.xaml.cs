using ChatTool.ViewModels.Main;
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

namespace ChatTool.Views.Main
{
    /// <summary>
    /// InputAreaView.xaml の相互作用ロジック
    /// </summary>
    public partial class InputAreaView : UserControl
    {
        public InputAreaView()
        {
            InitializeComponent();
            var viewModel = new InputViewModel();
            this.DataContext = viewModel;
        }
    }
}
