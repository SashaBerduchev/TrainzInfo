using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TrainzInfoWebGW;

bool DEBUG_MODE = true;
string conntring = "";
if(DEBUG_MODE == true)
{
    conntring = "https://localhost:44321/";
}else
{
    conntring = "https://trainzinfo.com.ua/";
}
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(conntring) });

await builder.Build().RunAsync();
