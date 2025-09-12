using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class VehicleDto
{
    [JsonPropertyName("vehicle_id")]
    public string VehicleId { get; set; } = string.Empty;

    [JsonPropertyName("reg_nr")]
    public string? RegNr { get; set; }
}