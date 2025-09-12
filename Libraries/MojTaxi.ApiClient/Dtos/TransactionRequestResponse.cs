using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class TransactionRequestResponse
{
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}