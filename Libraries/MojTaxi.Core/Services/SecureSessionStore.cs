using MojTaxi.Core.Abstractions;

namespace MojTaxi.Core.Services;

public sealed class SecureSessionStore : ISessionStore
{
    private const string ClientIdKey = "auth.client_id";
    private const string SessionIdKey = "auth.session_id";
    private const string DeviceIdKey = "auth.device_id";
    private const string ExpiresKey = "auth.expires_utc";

    public async Task SaveClientDataAsync(string clientId, string sessionId, string? deviceId, DateTime? expiresUtc)
    {
        await SecureStorage.SetAsync(ClientIdKey, clientId);
        await SecureStorage.SetAsync(SessionIdKey, sessionId);

        if (!string.IsNullOrWhiteSpace(deviceId))
            await SecureStorage.SetAsync(DeviceIdKey, deviceId);

        if (expiresUtc.HasValue)
            await SecureStorage.SetAsync(ExpiresKey, expiresUtc.Value.ToUniversalTime().ToString("O"));
    }

    public async Task<(string? ClientId, string? SessionId, string? DeviceId, DateTime? ExpiresUtc)> LoadClientDataAsync()
    {
        var clientId = await SecureStorage.GetAsync(ClientIdKey);
        var sessionId = await SecureStorage.GetAsync(SessionIdKey);
        var deviceId = await SecureStorage.GetAsync(DeviceIdKey);
        var expiresStr = await SecureStorage.GetAsync(ExpiresKey);

        DateTime? expiresUtc = null;
        if (!string.IsNullOrWhiteSpace(expiresStr) && DateTime.TryParse(expiresStr, out var dt))
            expiresUtc = DateTime.SpecifyKind(dt, DateTimeKind.Utc);

        return (clientId, sessionId, deviceId, expiresUtc);
    }

    public Task ClearClientDataAsync()
    {
        SecureStorage.Remove(ClientIdKey);
        SecureStorage.Remove(SessionIdKey);
        SecureStorage.Remove(DeviceIdKey);
        SecureStorage.Remove(ExpiresKey);
        return Task.CompletedTask;
    }

    public Task SaveAsync(string key, string value, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetAsync(string key, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
