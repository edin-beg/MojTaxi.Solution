using MojTaxi.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;

namespace MojTaxi.Shared.Services
{
    public class GpsService : IGpsService
    {
        public async Task<bool> IsGpsEnabledAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }

                if (status != PermissionStatus.Granted)
                    return false;

                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(
                        new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(5)));
                }

                return location != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
