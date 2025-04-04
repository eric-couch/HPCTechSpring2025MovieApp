using HPCTechSpring2025MovieApp.Client;
using HPCTechSpring2025MovieApp.Client.HttpRepo;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mzc5MTMyMUAzMjM5MmUzMDJlMzAzYjMyMzkzYm8yQkFvUEVUbEphSGVJdDBGbkVsblVsMURuUGdUbkxrb1hBTUhnai9XUEk9");

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// add the httpclient to the dependency injection container
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<IUserMoviesHttpRepository, UserMoviesHttpRepository>();

await builder.Build().RunAsync();
