using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class DriverUpdateApplicationDto
{
    [JsonPropertyName("file_name")]
    public string? FileName { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("file_hash")]
    public string? FileHash { get; set; }
}