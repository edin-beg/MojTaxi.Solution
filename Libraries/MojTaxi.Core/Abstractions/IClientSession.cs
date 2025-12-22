using MojTaxi.ApiClient.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{

    public interface IClientSession
    {
        ClientDto? Client { get; }
        IReadOnlyList<VehicleClientDto> Vehicles { get; }
        PaymentGatewaySettingsDto? PgwSettings { get; }
        string? SessionId { get; }
        DateTime? Expires { get; }

        bool IsLoggedIn { get; }

        /// <summary>
        /// Fired when login, restore or logout happens
        /// </summary>
        event Action? SessionChanged;

        void Set(ClientSessionResponse response);
        void Clear();
    }

}
