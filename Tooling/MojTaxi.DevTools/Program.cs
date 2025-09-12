using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MojTaxi.ApiClient;
using MojTaxi.Core;
using MojTaxi.Core.Abstractions;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using MojTaxi.ApiClient.Infrastructure;
using Refit;

// --- Minimalna in-memory implementacija ISessionStore samo za demo ---
static IAsyncPolicy<HttpResponseMessage> RetryPolicy() =>
    HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => (int)msg.StatusCode == 429)
        .WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * (retry + 1)));

var settings = new ApiSettings
{
    BaseUrl = "https://ssl.irget.se",
    Version = "V5",
    BuildNumber = "185",
    AppVersion = "1.3.1"
};

var host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton(settings);
        services.AddSingleton<ISessionStore, InMemorySessionStore>();

        services.AddRefitClient<IClientsApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .AddPolicyHandler(RetryPolicy());

        services.AddRefitClient<IDriversApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(settings.BaseUrl))
            .AddPolicyHandler(RetryPolicy());

        services.AddMojTaxiCore();
    })
    .Build();

// --- Demo pozivi ---
var sp = host.Services;

var clientAuth = sp.GetRequiredService<MojTaxi.Core.Abstractions.IClientAuthService>();
var driverAuth = sp.GetRequiredService<MojTaxi.Core.Abstractions.IDriverAuthService>();

Console.WriteLine("=== Clients login demo ===");
try
{
    var c = await clientAuth.LoginAsync("demo@example.com", "pass", "dev-1", settings.BuildNumber);
    Console.WriteLine($"Client session: {c.SessionId}");
}
catch (Exception ex)
{
    Console.WriteLine("Client login failed: " + ex.Message);
}

Console.WriteLine("=== Drivers orgs & login demo ===");
try
{
    var orgs = await driverAuth.GetOrganisationsAsync();
    Console.WriteLine($"Orgs: {orgs.Count}");
    // var login = await driverAuth.LoginAsync("1339", "driver_password", "new", orgs.First().OrganisationId, settings.AppVersion, settings.BuildNumber);
    // Console.WriteLine($"Driver session: {login.SessionId}");
}
catch (Exception ex)
{
    Console.WriteLine("Drivers demo failed: " + ex.Message);
}

sealed class InMemorySessionStore : ISessionStore
{
    private readonly Dictionary<string, string> _data = new();
    public Task SaveAsync(string key, string value, CancellationToken ct = default) { _data[key] = value; return Task.CompletedTask; }
    public Task<string?> GetAsync(string key, CancellationToken ct = default) => Task.FromResult(_data.TryGetValue(key, out var v) ? v : null);
    public Task RemoveAsync(string key, CancellationToken ct = default) { _data.Remove(key); return Task.CompletedTask; }
}