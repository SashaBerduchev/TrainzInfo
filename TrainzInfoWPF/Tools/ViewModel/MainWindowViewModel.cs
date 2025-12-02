using System.Collections.ObjectModel;
using System.Windows.Input;
using TrainzInfoShared.DTO;
using TrainzInfoWPF.Tools.Api;

namespace TrainzInfoWPF.Tools.ViewModel;

public class MainWindowViewModel : BaseViewModel
{
    private readonly ApiClient _api;
    private int _currentPage = 1;
    public ObservableCollection<NewsDTO> AllNews { get; set; } = new();
    public ObservableCollection<NewsDTO> FilteredNews { get; set; } = new();

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }

    public ICommand OpenNewsDetailsCommand { get; }
    public ICommand OpenLocomotivesCommand { get; }
    public ICommand OpenElectricTrainsCommand { get; }
    public ICommand OpenDieselTrainsCommand { get; }
    public ICommand OpenStationsCommand { get; }
    public MainWindowViewModel()
    {
        _api = new ApiClient();

        OpenNewsDetailsCommand = new RelayCommand<int>(OpenNewsDetails);
        OpenLocomotivesCommand = new RelayCommand(OpenLocomotives);
        OpenElectricTrainsCommand = new RelayCommand(OpenElectricTrains);
        OpenDieselTrainsCommand = new RelayCommand(OpenDieselTrains);
        OpenStationsCommand = new RelayCommand(OpenStations);

        LoadNews();
    }
    
    private async void LoadNews()
    {
        var data = await _api.GetNewsAsync(_currentPage);
        AllNews.Clear();
        foreach (var n in data)
            AllNews.Add(n);

        ApplyFilter();
    }
    
    private void ApplyFilter()
    {
        FilteredNews.Clear();
        foreach (var news in AllNews
                     .Where(x => string.IsNullOrEmpty(SearchText) || x.NameNews.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase)))
        {
            FilteredNews.Add(news);
        }
    }

    private void OpenNewsDetails(int id)
    {
        // Логіка відкриття деталей новини
    }

    private void OpenLocomotives() { /* навігація */ }
    private void OpenElectricTrains() { /* навігація */ }
    private void OpenDieselTrains() { /* навігація */ }
    private void OpenStations() { /* навігація */ }
    
    public async Task LoadMoreNewsAsync()
    {
        var data = await _api.GetNewsAsync(_currentPage); // API має підтримку пагінації
        foreach (var n in data)
            FilteredNews.Add(n);

        _currentPage++;
    }
}