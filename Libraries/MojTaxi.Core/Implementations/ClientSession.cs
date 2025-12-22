using MojTaxi.ApiClient.Dtos;
using MojTaxi.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Implementations
{

    public sealed class ClientSession : IClientSession
    {
        private ClientDto? _client;
        private List<VehicleClientDto> _vehicles = new();
        private PaymentGatewaySettingsDto? _pgw;
        private string? _sessionId;
        private DateTime? _expires;

        public ClientDto? Client => _client;
        public IReadOnlyList<VehicleClientDto> Vehicles => _vehicles;
        public PaymentGatewaySettingsDto? PgwSettings => _pgw;
        public string? SessionId => _sessionId;
        public DateTime? Expires => _expires;

        public bool IsLoggedIn =>
            _client != null && !string.IsNullOrWhiteSpace(_sessionId);

        public void Set(ClientSessionResponse response)
        {
            _client = response.Client;
            _vehicles = response.Vehicles ?? new();
            _pgw = response.PgwSettings;
            _sessionId = response.SessionId;
            _expires = response.Expires;
        }

        public void Clear()
        {
            _client = null;
            _vehicles.Clear();
            _pgw = null;
            _sessionId = null;
            _expires = null;
        }
    }
}
