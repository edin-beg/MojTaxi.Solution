using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class DriverSessionResponse
{
    [JsonPropertyName("driver_id")]
    public string DriverId { get; set; } = string.Empty;

    [JsonPropertyName("vehicle")]
    public VehicleDto? Vehicle { get; set; }

    [JsonPropertyName("session")]
    public SessionDto? Session { get; set; }

    [JsonPropertyName("organisation")]
    public OrganisationDto? Organisation { get; set; }
}