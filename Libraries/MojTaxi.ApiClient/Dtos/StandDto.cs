using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class StandDto
{
    [JsonPropertyName("stand_id")]
    public string StandId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }
}