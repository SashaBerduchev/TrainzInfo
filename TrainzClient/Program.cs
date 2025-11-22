using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrainzClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// 1. Визначаємо базовий URL для API
string apiBaseUrl;

// 1️⃣ Перевіряємо змінну оточення – це найзручніше, бо її можна встановити в CI/CD або у панелі хосту.
var envApi = Environment.GetEnvironmentVariable("TRAINZ_API_URL");

if (!string.IsNullOrWhiteSpace(envApi))
{
    apiBaseUrl = envApi;                          // 1. Якщо змінна задана – беремо з неї
}
else
{
    // 2️⃣ Якщо немає змінної, підставляємо значення за типом середовища
    switch (builder.HostEnvironment.Environment)
    {
        case "Development":
            apiBaseUrl = "https://localhost:44321/";   // ваш локальний API
            break;
        default:
            apiBaseUrl = "https://trainzinfo.com.ua/"; // продакшн‑домен
            break;
    }
}


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var handler = new HttpClientHandler { UseCookies = true };
    return new HttpClient(handler) { BaseAddress = new Uri(apiBaseUrl) };
});

await builder.Build().RunAsync();
