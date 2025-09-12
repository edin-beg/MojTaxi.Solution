using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class AlertResponseDto
{
    [JsonPropertyName("alert_id")]
    public string? AlertId { get; set; }
}