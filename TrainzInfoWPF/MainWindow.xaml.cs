using System.Windows;
using System.Windows.Controls;
using TrainzInfoWPF.Tools.ViewModel;

namespace TrainzInfoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isLoading = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void NewsScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;

            // Коли користувач приблизно внизу
            if (!_isLoading && scrollViewer.VerticalOffset + scrollViewer.ViewportHeight >= scrollViewer.ExtentHeight - 100)
            {
                _isLoading = true;

                if (DataContext is MainWindowViewModel vm)
                {
                    await vm.LoadMoreNewsAsync();
                }

                _isLoading = false;
            }
        }

    }

}