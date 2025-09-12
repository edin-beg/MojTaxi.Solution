using MojTaxi.ApiClient.Dtos;
using MojTaxi.Core.Models;

namespace MojTaxi.Core.Abstractions;

public interface IDriverAuthService
{
    Task<DriverAuthResult> LoginAsync(string taxiId, string password, string deviceId, string organisationId, string appVersion, string build, CancellationToken ct = default);
    Task<DriverSession> RefreshSessionAsync(string driverId, string sessionId, CancellationToken ct = default);
    Task<IReadOnlyList<OrganisationDto>> GetOrganisationsAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StandDto>> GetStandsAsync(string organisationId, string sessionId, CancellationToken ct = default);
    Task<bool> SendAlertAsync(string driverId, string sessionId, double lat, double lng, CancellationToken ct = default);
    Task LogoutAsync(CancellationToken ct = default);
}