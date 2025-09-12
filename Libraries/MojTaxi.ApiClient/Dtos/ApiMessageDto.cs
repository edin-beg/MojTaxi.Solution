using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos;

public sealed class ApiMessageDto
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

public sealed class TransactionStatusResponse
{
    [JsonPropertyName("transaction_id")]
    public string? TransactionId { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}