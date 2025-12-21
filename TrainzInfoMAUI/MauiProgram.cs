using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
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
                conntring = "https://localhost:44321/";
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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(conntring) });
            return builder.Build();
        }
    }
}
