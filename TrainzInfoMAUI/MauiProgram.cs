using Blazored.LocalStorage;
using CommunityToolkit.Maui;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using TrainzInfoMAUI.Tools;
using TrainzInfoWebGW.Tools;

namespace TrainzInfoMAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            bool DEBUG_MODE = false;
            string conntring = "";
            if (DEBUG_MODE == true)
            {
                conntring = "https://localhost:5001/";
            }
            else
            {
                conntring = "https://trainzinfo.com.ua/";
            }
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddMudServices();
            builder.UseMauiCommunityToolkit();
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(conntring) });
            builder.Services.AddSingleton(new AppConfig { ApiBaseUrl = conntring });
            return builder.Build();
        }
    }
}
