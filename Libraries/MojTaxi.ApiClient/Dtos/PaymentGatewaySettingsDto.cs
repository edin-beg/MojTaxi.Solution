using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class PaymentGatewaySettingsDto
{
    [JsonPropertyName("driver_id")]
    public string? DriverId { get; set; }

    [JsonPropertyName("merchant_id")]
    public string? MerchantId { get; set; }

    [JsonPropertyName("merchant_key")]
    public string? MerchantKey { get; set; }

    [JsonPropertyName("auth_token")]
    public string? AuthToken { get; set; }

    [JsonPropertyName("client_secret")]
    public string? ClientSecret { get; set; }

    [JsonPropertyName("authenticity_token")]
    public string? AuthenticityToken { get; set; }

    // "0" / "1"
    [JsonPropertyName("card_payment_enabled")]
    public string? CardPaymentEnabled { get; set; }
}
