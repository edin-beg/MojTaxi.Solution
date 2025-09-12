using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class PaymentGatewaySettingsDto
{
    [JsonPropertyName("authenticity_token")]
    public string? AuthenticityToken { get; set; }

    [JsonPropertyName("merchant_key")]
    public string? MerchantKey { get; set; }
}