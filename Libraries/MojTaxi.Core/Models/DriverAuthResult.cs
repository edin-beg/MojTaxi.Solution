using MojTaxi.ApiClient.Dtos;

namespace MojTaxi.Core.Models;

public sealed class DriverAuthResult
{
    public required string SessionId { get; init; }
    public required string DriverId { get; init; }
    public DriverDto? Driver { get; init; }
    public OrganisationDto? Organisation { get; init; }
    public VehicleDto? Vehicle { get; init; }
    public DriverUpdateApplicationDto? Application { get; init; }
}