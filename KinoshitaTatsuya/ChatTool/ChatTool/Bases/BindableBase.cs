using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ChatTool.Bases
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string? propertyName = null)
        {                        
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetProperty<T>(ref T strage,T value,[CallerMemberName]string? propertyName = null)
        {
            if (object.Equals(strage, value)) return;

            strage = value;
            OnPropertyChanged(propertyName);
        }
    }
}
