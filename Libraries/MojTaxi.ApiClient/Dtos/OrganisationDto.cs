using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class OrganisationDto
{
    [JsonPropertyName("organisation_id")]
    public string OrganisationId { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [JsonPropertyName("driver_response_timeout")]
    public string? DriverResponseTimeout { get; set; }
}