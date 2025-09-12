using MojTaxi.ApiClient.Dtos;

namespace MojTaxi.Core.Models;

// --- Driver ---
public sealed class DriverSession
{
    public required string DriverId { get; init; }
    public required string SessionId { get; init; }
    public VehicleDto? Vehicle { get; init; }
    public OrganisationDto? Organisation { get; init; }
}