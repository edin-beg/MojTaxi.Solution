using MojTaxi.ApiClient.Converters;
using System;
using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class ClientDto
{
    [JsonPropertyName("client_id")]
    public string? ClientId { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("zip")]
    public string? Zip { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("birth_date")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? BirthDate { get; set; }

    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("created")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? Created { get; set; }

    [JsonPropertyName("modified")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime? Modified { get; set; }

    [JsonPropertyName("active")]
    public string? Active { get; set; }

    [JsonPropertyName("activated")]
    public string? Activated { get; set; }

    [JsonPropertyName("activation_id")]
    public string? ActivationId { get; set; }

    [JsonPropertyName("promo_ride_used")]
    public string? PromoRideUsed { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [JsonPropertyName("visible_latitude")]
    public string? VisibleLatitude { get; set; }

    [JsonPropertyName("visible_longitude")]
    public string? VisibleLongitude { get; set; }

    [JsonPropertyName("static_client")]
    public string? StaticClient { get; set; }

    [JsonPropertyName("uri")]
    public string? Uri { get; set; }

    [JsonPropertyName("booking_timeout")]
    public string? BookingTimeout { get; set; }

    [JsonPropertyName("booking_delay")]
    public string? BookingDelay { get; set; }
}
