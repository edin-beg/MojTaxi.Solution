using MojTaxi.Core.Abstractions;

namespace MojTaxi.StaticClientApp.Services;

public sealed class SecureStorageSessionStore : ISessionStore
{
    public Task SaveAsync(string key, string value, CancellationToken ct = default)
        => SecureStorage.Default.SetAsync(key, value);

    public async Task<string?> GetAsync(string key, CancellationToken ct = default)
        => await SecureStorage.Default.GetAsync(key);

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        SecureStorage.Default.Remove(key);
        return Task.CompletedTask;
    }

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