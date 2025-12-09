using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Devices.Sensors;

namespace MojTaxi.Core.Models
{

    /// <summary>
    /// Pomoćne metode za rad sa lokacijama.
    /// </summary>
    public static class LocationHelper
    {
        // Poluprečnik Zemlje u metrima (WGS84 approx)
        private const double EarthRadius = 6378137;

        /// <summary>
        /// Vrati random lokaciju u krugu od maxDistanceMeters oko centar lokacije.
        /// </summary>
        public static Location GetRandomLocationNear(
            Location center,
            double maxDistanceMeters,
            Random rng)
        {
            // Udaljenost [0, max] i nasumičan pravac [0, 2π)
            double distance = rng.NextDouble() * maxDistanceMeters;
            double bearing = rng.NextDouble() * 2.0 * Math.PI;

            double lat1 = DegreesToRadians(center.Latitude);
            double lon1 = DegreesToRadians(center.Longitude);

            double angularDistance = distance / EarthRadius;

            double lat2 = Math.Asin(
                Math.Sin(lat1) * Math.Cos(angularDistance) +
                Math.Cos(lat1) * Math.Sin(angularDistance) * Math.Cos(bearing));

            double lon2 = lon1 + Math.Atan2(
                Math.Sin(bearing) * Math.Sin(angularDistance) * Math.Cos(lat1),
                Math.Cos(angularDistance) - Math.Sin(lat1) * Math.Sin(lat2));

            return new Location(RadiansToDegrees(lat2), RadiansToDegrees(lon2));
        }

        private static double DegreesToRadians(double deg) => deg * Math.PI / 180.0;
        private static double RadiansToDegrees(double rad) => rad * 180.0 / Math.PI;
    }
}
