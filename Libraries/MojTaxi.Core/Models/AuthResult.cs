using MojTaxi.ApiClient.Dtos;

namespace MojTaxi.Core.Models;

// --- Client ---
public sealed class AuthResult
{
    public required string SessionId { get; init; }
    public ClientDto? Client { get; init; }
    public List<VehicleDto>? Vehicles { get; init; }
    public PaymentGatewaySettingsDto? PaymentGatewaySettings { get; init; }
}
