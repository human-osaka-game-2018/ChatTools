using ChatTool.Infrastructure.Database;
using ChatTool.Models.DomainObjects;
using ChatTool.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;

namespace ChatTool.ViewModels.Main
{
    class ChannelViewModel
    {
        public ChannelViewModel()
        {
            var channelService = new ChannelService();
            var user = LoginService.User;
            if(user != null){
            UserName = user.Name;
            channelService.GetAvailableByUserid(Channels,user.Id);
                IconPath = user.IconPath;
            }
        }

        public string IconPath { get; set; } = "";
        private Channel selectItem  = new Channel();
        public Channel? SelectItem { 
            get { return selectItem; } 
            set {
                if (null == value) return;
                selectItem = value;
                ChannelSelected(); 
            } 
        }

        private string userName = "";
        public string UserName { get { return userName; } set { userName = value; } }
        #region
        private ObservableCollection<Channel> _Channels = new ObservableCollection<Channel>();
        public ObservableCollection<Channel> Channels
        {
            get { return _Channels; }
            set
            {
                if (value != this._Channels)
                    _Channels = value;
                this.SetPropertyChanged("Channels");
            }
        }
        #endregion
        public event PropertyChangedEventHandler? PropertyChanged;
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChannelSelected()
        {
            if (null != SelectItem)
            {
                SelectChannelService.CallMessageLogs(SelectItem.Id);
            }
        }
    }
}
