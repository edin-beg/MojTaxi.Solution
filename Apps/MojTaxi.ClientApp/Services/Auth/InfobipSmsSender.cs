using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MojTaxi.ClientApp.Services.Auth
{

    public sealed class InfobipSmsSender : IOtpSender
    {
        private readonly HttpClient _httpClient;

        // TODO: prebaci u ApiSettings
        private const string BaseUrl = "https://api.infobip.com";
        private const string ApiKey = "ea5422bac1f31c97627a3e6d80ebf450-62f4c29f-0a51-41aa-a35b-80f53e46c4d8";
        private const string Sender = "IT Craft BA";

        public InfobipSmsSender(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("App", ApiKey);
        }

        public async Task SendAsync(string phoneNumber, string otp)
        {
            var payload = new
            {
                messages = new[]
                {
                new
                {
                    from = Sender,
                    destinations = new[]
                    {
                        new { to = phoneNumber }
                    },
                    text = $"MojTaxi kod za prijavu: {otp}"
                }
            }
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/sms/2/text/advanced", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new InvalidOperationException($"Infobip SMS error: {error}");
            }
        }
    }
}
