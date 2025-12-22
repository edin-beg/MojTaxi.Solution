using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class ClientLoginResponse
{
    [JsonPropertyName("session_id")]
    public string SessionId { get; set; } = string.Empty;

    [JsonPropertyName("client")]
    public ClientDto? Client { get; set; }

    [JsonPropertyName("vehicles")]
    public List<VehicleClientDto>? Vehicles { get; set; }

    [JsonPropertyName("pgw_settings")]
    public PaymentGatewaySettingsDto? PaymentGatewaySettings { get; set; }
}