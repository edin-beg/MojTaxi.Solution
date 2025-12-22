namespace MojTaxi.Core.Abstractions;

public interface ISessionStore
{
    Task SaveAsync(string key, string value, CancellationToken ct = default);
    Task<string?> GetAsync(string key, CancellationToken ct = default);
    Task RemoveAsync(string key, CancellationToken ct = default);

    Task SaveClientDataAsync(string clientId, string sessionId, string? deviceId, DateTime? expiresUtc);
    Task<(string? ClientId, string? SessionId, string? DeviceId, DateTime? ExpiresUtc)> LoadClientDataAsync();
    Task ClearClientDataAsync();
}