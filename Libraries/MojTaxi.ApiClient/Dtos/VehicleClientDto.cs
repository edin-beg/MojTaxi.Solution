using MojTaxi.ApiClient.Converters;
using System;
using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class VehicleClientDto
{
    [JsonPropertyName("taxi_nr")]
    public string? TaxiNr { get; set; }

    [JsonPropertyName("organisation_id")]
    public string? OrganisationId { get; set; }

    [JsonPropertyName("allowed_distance_from_organisation")]
    public string? AllowedDistanceFromOrganisation { get; set; }

    [JsonPropertyName("location_id")]
    public string? LocationId { get; set; }

    [JsonPropertyName("vehicle_id")]
    public string? VehicleId { get; set; }

    [JsonPropertyName("driver_id")]
    public string? DriverId { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [JsonPropertyName("last_seen")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? LastSeen { get; set; }

    [JsonPropertyName("distance_from_organisation")]
    public string? DistanceFromOrganisation { get; set; }

    // "0" / "1"
    [JsonPropertyName("in_use")]
    public string? InUse { get; set; }

    // "0" / "1"
    [JsonPropertyName("accessibility_enabled")]
    public string? AccessibilityEnabled { get; set; }

    [JsonPropertyName("response_timeout")]
    public string? ResponseTimeout { get; set; }

    // "false" / "true" (string)
    [JsonPropertyName("card_payments_enabled")]
    public string? CardPaymentsEnabled { get; set; }
}
