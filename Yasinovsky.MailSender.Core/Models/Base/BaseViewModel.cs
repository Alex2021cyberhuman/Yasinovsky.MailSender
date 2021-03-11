using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Yasinovsky.MailSender.Core.Models.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null, bool checkEqual = true)
        {
            if (checkEqual && property is not null && property.Equals(value))
                return;
            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
