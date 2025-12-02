using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TrainzInfoWPF.Tools.ValueConverters;

public class Base64ToImageSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var dataUri = value as string;
        if (string.IsNullOrEmpty(dataUri)) return null;

        try
        {
            // data:image/jpeg;base64,......
            var base64Data = dataUri.Substring(dataUri.IndexOf(",") + 1);
            var bytes = System.Convert.FromBase64String(base64Data);

            using (var ms = new MemoryStream(bytes))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze();
                return image;
            }
        }
        catch
        {
            return null;
        }
    }
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}