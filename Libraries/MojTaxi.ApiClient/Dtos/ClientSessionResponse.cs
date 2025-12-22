using MojTaxi.ApiClient.Converters;
using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;


public sealed class ClientSessionResponse
{
    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("device_id")]
    public string? DeviceId { get; set; }

    [JsonPropertyName("created")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? Created { get; set; }

    [JsonPropertyName("expires")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? Expires { get; set; }

    [JsonPropertyName("beta")]
    public string? Beta { get; set; }

    [JsonPropertyName("client")]
    public ClientDto? Client { get; set; }

    [JsonPropertyName("vehicles")]
    public List<VehicleClientDto>? Vehicles { get; set; }

    [JsonPropertyName("pgw_settings")]
    public PaymentGatewaySettingsDto? PgwSettings { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }
}