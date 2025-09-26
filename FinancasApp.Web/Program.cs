using FinancasApp.Web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Configuração da biblioteca HTTP CLIENT (chamadas para serviços de API)
builder.Services.AddScoped(sp => new HttpClient
{
    //Endereço da API backend
    BaseAddress = new Uri("http://localhost:5098/")
});

await builder.Build().RunAsync();



