using Microsoft.Extensions.Logging;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;

namespace MojTaxi.Core.Implementations;

public sealed class ClientAuthService : IClientAuthService
{
    private readonly IClientsApi _api;
    private readonly ApiSettings _settings;
    private readonly ISessionStore _store;
    private readonly ILogger<ClientAuthService> _log;

    private const string ClientSessionKey = "client.session_id";
    private const string ClientIdKey = "client.client_id";

    public ClientAuthService(IClientsApi api, ApiSettings settings, ISessionStore store, ILogger<ClientAuthService> log)
    {
        _api = api;
        _settings = settings;
        _store = store;
        _log = log;
    }

    public async Task<AuthResult> LoginAsync(string email, string password, string deviceId, string buildNumber, CancellationToken ct = default)
    {
        var body = new Dictionary<string, object>
        {
            ["email"] = email,
            ["password"] = password,
            ["device_id"] = deviceId,
            ["build_number"] = buildNumber
        };

        var res = await _api.LoginClientAsync(_settings.Version, body);
        if (string.IsNullOrWhiteSpace(res.SessionId))
            throw new InvalidOperationException("SessionId nije vraćen iz API-ja.");

        await _store.SaveAsync(ClientSessionKey, res.SessionId, ct);
        if (!string.IsNullOrWhiteSpace(res.Client?.ClientId))
            await _store.SaveAsync(ClientIdKey, res.Client!.ClientId!, ct);

        return new AuthResult
        {
            SessionId = res.SessionId,
            Client = res.Client,
            Vehicles = res.Vehicles ?? new(),
            PaymentGatewaySettings = res.PaymentGatewaySettings
        };
    }

    public async Task<ClientSession> RefreshSessionAsync(string clientId, string sessionId, string? buildNumber = null, bool excludeVehicles = false, CancellationToken ct = default)
    {
        var res = await _api.GetSessionAsync(_settings.Version, clientId, sessionId, buildNumber, excludeVehicles);
        if (!string.IsNullOrWhiteSpace(res.SessionId))
            await _store.SaveAsync(ClientSessionKey, res.SessionId!, ct);

        return new ClientSession
        {
            SessionId = res.SessionId,
            Client = res.Client,
            Vehicles = res.Vehicles ?? new()
        };
    }

    public async Task<bool> StartRecoveryAsync(string email, CancellationToken ct = default)
    {
        var body = new Dictionary<string, object> { ["email"] = email };
        var r = await _api.StartRecoveryAsync(_settings.Version, body);
        return string.IsNullOrEmpty(r.Error);
    }

    public async Task<bool> CompleteRecoveryAsync(string token, string newPassword, CancellationToken ct = default)
    {
        var body = new Dictionary<string, object> { ["password"] = newPassword };
        var r = await _api.CompleteRecoveryAsync(_settings.Version, token, body);
        return string.IsNullOrEmpty(r.Error);
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        await _store.RemoveAsync(ClientSessionKey, ct);
        await _store.RemoveAsync(ClientIdKey, ct);
        _log.LogInformation("Client logout: session cleared.");
    }
}