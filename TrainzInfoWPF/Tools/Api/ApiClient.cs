using System.Net.Http;
using System.Net.Http.Json;
using TrainzInfoShared.DTO;

namespace TrainzInfoWPF.Tools.Api;

public class ApiClient
{
    private readonly HttpClient _client;
    public ApiClient()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://trainzinfo.com.ua") // твій API URL
        };
    }
    public async Task<List<LocomotiveDTO>> GetLocomotivesAsync()
    {
        return await _client.GetFromJsonAsync<List<LocomotiveDTO>>("api/locomotives/getlocomotives");
    }

    // Отримати всі новини
    public async Task<List<NewsDTO>> GetNewsAsync(int page)
    {
        return await _client.GetFromJsonAsync<List<NewsDTO>>($"api/news/getnews?page={page}");
    }

    // Відправити новий локомотив на сервер
    public async Task<bool> CreateLocomotiveAsync(LocomotiveDTO dto)
    {
        var response = await _client.PostAsJsonAsync("api/locomotives/create", dto);
        return response.IsSuccessStatusCode;
    }
}