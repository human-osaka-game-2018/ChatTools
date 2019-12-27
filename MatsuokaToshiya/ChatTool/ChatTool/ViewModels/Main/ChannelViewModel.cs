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
            UserName = channelService.ParticipatedUser(Channels);
        }
        private Channel _SelectItem  = new Channel();
        public Channel? SelectItem { 
            get { return _SelectItem; } 
            set {
                if (null == value) return;
                _SelectItem = value;
                ChannelSelected(); 
            } 
        }
        private string _UserName = "";
        public string UserName { get { return _UserName; } set { _UserName = value; } }
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
        public event PropertyChangedEventHandler? PropertyChanged;
        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChannelSelected()
        {
            if (null != SelectItem)
            {
                Debug.Write(SelectItem.Name + "Select\n");
                SelectChannelService.CallMessageLogs(SelectItem.Id);
            }
        }
    }
}
