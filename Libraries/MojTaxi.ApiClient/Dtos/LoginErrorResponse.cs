using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MojTaxi.ApiClient.Dtos
{
    public sealed class LoginErrorResponse
    {
        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }

}
