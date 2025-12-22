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
    })
    .Build();

sealed class InMemorySessionStore : ISessionStore
{
    private readonly Dictionary<string, string> _data = new();
    public Task SaveAsync(string key, string value, CancellationToken ct = default) { _data[key] = value; return Task.CompletedTask; }
    public Task<string?> GetAsync(string key, CancellationToken ct = default) => Task.FromResult(_data.TryGetValue(key, out var v) ? v : null);
    public Task RemoveAsync(string key, CancellationToken ct = default) { _data.Remove(key); return Task.CompletedTask; }

    public Task SaveClientDataAsync(string clientId, string sessionId, string? deviceId, DateTime? expiresUtc)
    {
        throw new NotImplementedException();
    }

    public Task<(string? ClientId, string? SessionId, string? DeviceId, DateTime? ExpiresUtc)> LoadClientDataAsync()
    {
        throw new NotImplementedException();
    }

    public Task ClearClientDataAsync()
    {
        throw new NotImplementedException();
    }
}