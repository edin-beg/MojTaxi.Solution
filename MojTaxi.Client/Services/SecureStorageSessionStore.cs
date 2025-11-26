using MojTaxi.Core.Abstractions;

namespace MojTaxi.Client.Services;

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
}