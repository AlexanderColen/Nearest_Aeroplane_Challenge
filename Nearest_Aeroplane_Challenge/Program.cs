using Nearest_Aeroplane_Challenge.Models;
using Nearest_Aeroplane_Challenge.Services;
using Nearest_Aeroplane_Challenge.Utility;
using System;
using System.Threading.Tasks;

namespace Nearest_Aeroplane_Challenge
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*
             * Check if arguments are provided and try to parse them as the longitude and latitude to look for.
             * First argument should be Longitude. (North is positive, South is negative.)
             * Second argument should be Latitude. (East is positive, West is negative.)
             */
            if (args.Length > 0)
            {
                bool validLongitude = decimal.TryParse(args[0], out decimal longitude);
                bool validLatitude = decimal.TryParse(args[1], out decimal latitude);
                // Try to parse and return an error message if this fails.
                if (!validLongitude || !validLatitude)
                {
                    Console.WriteLine("Failed to parse the command-line arguments.\n" +
                        "Please make sure that the arguments are valid.\n" +
                        "First argument should be Longitude. (North is positive, South is negative.)\n" +
                        "Second argument should be Latitude. (East is positive, West is negative.)");
                }

                Console.WriteLine($"Looking for States near the {longitude} - {latitude}");
                var apiFetcher = new ApiFetcher();
                var distanceCalculation = new DistanceCalculation();

                StatesResponse response = await apiFetcher.GetStatesNearCoordinates(longitude, latitude);

                var shortestDistance = double.MaxValue;
                State foundState = null;
                // Calculate the distances for every state and return the shortest.
                foreach (var state in response.ConvertedStates)
                {
                    var distance = distanceCalculation.CalculateGeodesicDistance(longitude, latitude, state.Longitude, state.Latitude);
                    // Overwrite the new shortest before checking other States.
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        foundState = state;
                    }
                }

                Console.WriteLine($"Closest State to the given coordinates ({longitude}, {latitude}):\n" +
                    $"Geodesic distance: {shortestDistance}\n" +
                    $"Callsign: {foundState.CallSign}\n" +
                    $"Longitude: {foundState.Longitude}\n" +
                    $"Latitude: {foundState.Latitude}\n" +
                    $"Geometric Altitude: {foundState.GeoAltitude}\n" +
                    $"Country of Origin: {foundState.OriginCountry}\n" +
                    $"ICAO24 ID: {foundState.ICAO24}");
            }
            // Otherwise test it out with the two given challenge inputs.
            else
            {
                var apiFetcher = new ApiFetcher();
                var distanceCalculation = new DistanceCalculation();

                /*
                 * Eiffel Tower
                 * Longitude: 48.8584 N
                 * Latitude: 2.2945 E
                 */
                Console.WriteLine("Looking for States near the Eiffel Tower...");
                var longitude = 48.8584M;
                var latitude = 2.2945M;
                StatesResponse response = await apiFetcher.GetStatesNearCoordinates(longitude, latitude);

                var shortestDistance = double.MaxValue;
                State foundState = null;
                // Calculate the distances for every state and return the shortest.
                foreach (var state in response.ConvertedStates)
                {
                    var distance = distanceCalculation.CalculateGeodesicDistance(longitude, latitude, state.Longitude, state.Latitude);
                    // Overwrite the new shortest before checking other States.
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        foundState = state;
                    }
                }

                Console.WriteLine($"Closest State to the Eiffel Tower:\n" +
                    $"Geodesic distance: {shortestDistance}\n" +
                    $"Callsign: {foundState.CallSign}\n" +
                    $"Longitude: {foundState.Longitude}\n" +
                    $"Latitude: {foundState.Latitude}\n" +
                    $"Geometric Altitude: {foundState.GeoAltitude}\n" +
                    $"Country of Origin: {foundState.OriginCountry}\n" +
                    $"ICAO24 ID: {foundState.ICAO24}");

                /*
                 * John F. Kennedy Airport
                 * Longitude: 40.6413 N
                 * Latitude: 73.7781 W
                 */
                Console.WriteLine("Looking for States near the John F. Kennedy Airport...");
                longitude = 40.6413M;
                latitude = -73.7781M;
                response = await apiFetcher.GetStatesNearCoordinates(longitude, latitude);

                shortestDistance = double.MaxValue;
                foundState = null;
                // Calculate the distances for every state and return the shortest.
                foreach (var state in response.ConvertedStates)
                {
                    var distance = distanceCalculation.CalculateGeodesicDistance(longitude, latitude, state.Longitude, state.Latitude);
                    // Overwrite the new shortest before checking other States.
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        foundState = state;
                    }
                }

                Console.WriteLine($"Closest State to the John F. Kennedy Airport:\n" +
                    $"Geodesic distance: {shortestDistance}\n" +
                    $"Callsign: {foundState.CallSign}\n" +
                    $"Longitude: {foundState.Longitude}\n" +
                    $"Latitude: {foundState.Latitude}\n" +
                    $"Geometric Altitude: {foundState.GeoAltitude}\n" +
                    $"Country of Origin: {foundState.OriginCountry}\n" +
                    $"ICAO24 ID: {foundState.ICAO24}");
            }
        }
    }
}