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
    class ChannelViewModel: INotifyPropertyChanged
    {
        public ChannelViewModel()
        {
            var channelDao = new ChannelDAO();
            if (null == LoginService.user || null == LoginService.user.Name) return;
            _UserName = LoginService.user.Name;
            channelDao.ParticipatedUser(Channels, LoginService.user.Id);
        }
        public Channel? SelectItem { get; set; }
        private string _UserName = "";
        public string UserName { get { return _UserName; }  }
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
        private void NotifyPropertyChanged(string adress)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(adress));
        }
        public void SetPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ChannelSelected()
        {
            if (null != SelectItem)
            {
                Debug.Write(SelectItem.Name + "Select\n");
                MessageLogService.Fire(SelectItem.Id);
            }
        }
    }
}
