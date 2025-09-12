using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class DriverDto
{
    [JsonPropertyName("driver_id")]
    public string DriverId { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("taxi_id")]
    public string? TaxiId { get; set; }

    [JsonPropertyName("organisation_id")]
    public string? OrganisationId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("organisation")]
    public OrganisationDto? Organisation { get; set; }

    [JsonPropertyName("vehicle")]
    public VehicleDto? Vehicle { get; set; }
}