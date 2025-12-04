using System;
using System.Collections.Generic;
using System.Text;

namespace MojTaxi.Core.Abstractions
{
    public interface IAppStatusService
    {
        bool HasInternet { get; }
        bool HasGps { get; }

        event Action<bool>? InternetStatusChanged;
        event Action<bool>? GpsStatusChanged;

        void StartMonitoring();
    }
}
