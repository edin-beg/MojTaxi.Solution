using MojTaxi.Core.Abstractions;
using MojTaxi.Core.Models;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace MojTaxi.Core.Implementations
{
    public class RemoteErrorSender : IRemoteErrorSender
    {
        private readonly HttpClient _http;

        public RemoteErrorSender(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> SendAsync(ErrorEntry entry)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/api/app/log-error", entry);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
