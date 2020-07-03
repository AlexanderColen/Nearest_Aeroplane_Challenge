using System;

namespace Nearest_Aeroplane_Challenge.Utility
{
    public class DistanceCalculation
    {
        /// <summary>
        /// Calculate the direct distance between two coordinates.
        /// </summary>
        /// <param name="longitude_a">The Longitude of point A.</param>
        /// <param name="latitude_a">The Latitude of point A.</param>
        /// <param name="longitude_b">The Longitude of point B.</param>
        /// <param name="latitude_b">The Latitude of point B.</param>
        /// <returns>The distance between point A and point B.</returns>
        public double CalculateDirectDistance(decimal longitude_a, decimal latitude_a, decimal? longitude_b, decimal? latitude_b)
        {
            // Return maximum value if point B was missing coordinates.
            if (longitude_b == null || latitude_b == null)
            {
                return double.MaxValue;
            }

            return Math.Sqrt(Math.Pow((double)(longitude_a - longitude_b), 2) + Math.Pow((double)(latitude_a - latitude_b), 2));
        }

        /// <summary>
        /// Calculate the geodesic distance between two coordinates.
        /// </summary>
        /// <param name="longitude_a">The Longitude of point A.</param>
        /// <param name="latitude_a">The Latitude of point A.</param>
        /// <param name="longitude_b">The Longitude of point B.</param>
        /// <param name="latitude_b">The Latitude of point B.</param>
        /// <returns>The distance between point A and point B.</returns>
        public double CalculateGeodesicDistance(decimal longitude_a, decimal latitude_a, decimal? longitude_b, decimal? latitude_b)
        {
            // Return maximum value if point B was missing coordinates.
            if (longitude_b == null || latitude_b == null)
            {
                return double.MaxValue;
            }

            var delta_long = 0.0;
            if (longitude_a > longitude_b)
            {
                delta_long = (double)(Math.Abs(longitude_a) - Math.Abs((decimal)longitude_b));
            }
            else
            {
                delta_long = (double)(Math.Abs((decimal)longitude_b) - Math.Abs(longitude_a));
            }

            return 6378.137 * Math.Acos(Math.Sin((double)latitude_a) * Math.Sin((double)latitude_b) + Math.Cos((double)latitude_a) * Math.Cos((double)latitude_b) * Math.Cos(delta_long));
        }
    }
}