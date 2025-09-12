using MojTaxi.Core.Models;

namespace MojTaxi.Core.Abstractions;

public interface IClientAuthService
{
    Task<AuthResult> LoginAsync(string email, string password, string deviceId, string buildNumber, CancellationToken ct = default);
    Task<ClientSession> RefreshSessionAsync(string clientId, string sessionId, string? buildNumber = null, bool excludeVehicles = false, CancellationToken ct = default);
    Task<bool> StartRecoveryAsync(string email, CancellationToken ct = default);
    Task<bool> CompleteRecoveryAsync(string token, string newPassword, CancellationToken ct = default);
    Task LogoutAsync(CancellationToken ct = default);
}