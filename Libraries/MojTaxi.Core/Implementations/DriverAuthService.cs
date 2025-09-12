using Microsoft.Extensions.Logging;
using MojTaxi.ApiClient;
using MojTaxi.ApiClient.Dtos;
using MojTaxi.ApiClient.Infrastructure;
using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;

namespace MojTaxi.Core.Implementations;

public sealed class DriverAuthService : IDriverAuthService
{
    private readonly IDriversApi _api;
    private readonly ApiSettings _settings;
    private readonly ISessionStore _store;
    private readonly ILogger<DriverAuthService> _log;

    private const string DriverSessionKey = "driver.session_id";
    private const string DriverIdKey = "driver.driver_id";

    public DriverAuthService(IDriversApi api, ApiSettings settings, ISessionStore store, ILogger<DriverAuthService> log)
    {
        _api = api;
        _settings = settings;
        _store = store;
        _log = log;
    }

    public async Task<DriverAuthResult> LoginAsync(string taxiId, string password, string deviceId, string organisationId, string appVersion, string build, CancellationToken ct = default)
    {
        var body = new Dictionary<string, object>
        {
            ["taxi_id"] = taxiId,
            ["password"] = password,
            ["device_id"] = deviceId,
            ["organisation_id"] = organisationId,
            ["version"] = appVersion,
            ["build"] = build
        };

        var res = await _api.LoginDriverAsync(_settings.Version, body);
        var sessionId = res.Session?.SessionId ?? throw new InvalidOperationException("Driver sessionId nije vraćen.");
        var driverId = res.DriverId;

        await _store.SaveAsync(DriverSessionKey, sessionId, ct);
        await _store.SaveAsync(DriverIdKey, driverId, ct);

        return new DriverAuthResult
        {
            SessionId = sessionId,
            DriverId = driverId,
            Driver = new DriverDto
            {
                DriverId = res.DriverId,
                FirstName = res.FirstName,
                LastName = res.LastName,
                Email = res.Email,
                Phone = res.Phone,
                TaxiId = res.TaxiId,
                OrganisationId = res.OrganisationId,
                Status = res.Status,
                Organisation = res.Organisation,
                Vehicle = res.Vehicle
            },
            Organisation = res.Organisation,
            Vehicle = res.Vehicle,
            Application = res.Application
        };
    }

    public async Task<DriverSession> RefreshSessionAsync(string driverId, string sessionId, CancellationToken ct = default)
    {
        var res = await _api.GetDriverSessionAsync(_settings.Version, driverId, sessionId);
        var newSess = res.Session?.SessionId ?? sessionId;
        await _store.SaveAsync(DriverSessionKey, newSess, ct);

        return new DriverSession
        {
            DriverId = res.DriverId,
            SessionId = newSess,
            Vehicle = res.Vehicle,
            Organisation = res.Organisation
        };
    }

    public async Task<IReadOnlyList<OrganisationDto>> GetOrganisationsAsync(CancellationToken ct = default)
        => await _api.GetOrganisationsAsync(_settings.Version);

    public async Task<IReadOnlyList<StandDto>> GetStandsAsync(string organisationId, string sessionId, CancellationToken ct = default)
        => await _api.GetStandsAsync(_settings.Version, organisationId, sessionId);

    public async Task<bool> SendAlertAsync(string driverId, string sessionId, double lat, double lng, CancellationToken ct = default)
    {
        var body = new Dictionary<string, object>
        {
            ["session_id"] = sessionId,
            ["latitude"] = lat.ToString(System.Globalization.CultureInfo.InvariantCulture),
            ["longitude"] = lng.ToString(System.Globalization.CultureInfo.InvariantCulture)
        };

        var r = await _api.SendAlertAsync(_settings.Version, driverId, body);
        return !string.IsNullOrEmpty(r.AlertId);
    }

    public async Task LogoutAsync(CancellationToken ct = default)
    {
        await _store.RemoveAsync(DriverSessionKey, ct);
        await _store.RemoveAsync(DriverIdKey, ct);
        _log.LogInformation("Driver logout: session cleared.");
    }
}