using MojTaxi.ApiClient.Dtos;

namespace MojTaxi.Core.Models;

public sealed class ClientSession
{
    public string? SessionId { get; init; }
    public ClientDto? Client { get; init; }
    public List<VehicleDto>? Vehicles { get; init; }
}