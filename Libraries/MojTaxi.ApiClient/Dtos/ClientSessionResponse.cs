using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class ClientSessionResponse
{
    [JsonPropertyName("client")]
    public ClientDto? Client { get; set; }

    [JsonPropertyName("vehicles")]
    public List<VehicleDto>? Vehicles { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }
}