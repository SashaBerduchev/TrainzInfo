using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrainzInfoWPF.Tools.ViewModel;

public class BaseViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    // Викликаємо при зміні властивості, щоб UI оновився
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}