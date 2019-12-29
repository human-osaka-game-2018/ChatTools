using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF_Core.ViewModels
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetValue<T>(ref T variable, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(variable, value)) return false;

            variable = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
