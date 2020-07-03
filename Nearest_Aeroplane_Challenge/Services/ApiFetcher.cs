using Nearest_Aeroplane_Challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nearest_Aeroplane_Challenge.Services
{
    public class ApiFetcher
    {
        private static readonly string OPENSKY_API_URL = "https://opensky-network.org/api/states/all";
        private static readonly int EXTRA_DEGREES = 5;

        /// <summary>
        /// Find States that are near certain coordinates.
        /// </summary>
        /// <param name="longitude">The Longitude to look near to.</param>
        /// <param name="latitude">The Latitude to look near to.</param>
        /// <returns>The response from the API as a StatesResponse object.</returns>
        public async Task<StatesResponse> GetStatesNearCoordinates(decimal longitude, decimal latitude)
        {
            // Define the box to look in by adding extra degrees around the given longitude and latitude.
            var longitude_min = longitude - EXTRA_DEGREES;
            var longitude_max = longitude + EXTRA_DEGREES;
            var latitude_min = latitude - EXTRA_DEGREES;
            var latitude_max = latitude + EXTRA_DEGREES;

            // Query the API with the given data.
            var statesResponse = await QueryAPI(longitude_min, longitude_max, latitude_min, latitude_max);

            var extra_iterations = 1;
            // If there are no States found, add extra degrees to the search radius and try again.
            while (statesResponse.States == null)
            {
                Console.WriteLine($"No results. Trying extra iteration {extra_iterations}...");
                longitude_min -= EXTRA_DEGREES;
                longitude_max += EXTRA_DEGREES;
                latitude_min -= EXTRA_DEGREES;
                latitude_max += EXTRA_DEGREES;

                statesResponse = await QueryAPI(longitude_min, longitude_max, latitude_min, latitude_max);
                extra_iterations++;
            }

            // Map to actual states since the State JSON uses unnamed attributes.
            return MapObjectsToStates(statesResponse);
        }

        /// <summary>
        /// Query the OpenSky API to find states that within a certain box.
        /// </summary>
        /// <param name="long_min">The minimun longitude of the box.</param>
        /// <param name="long_max">The maximum longitude of the box.</param>
        /// <param name="lat_min">The minimun latitude of the box.</param>
        /// <param name="lat_max">The maximum latitude of the box.</param>
        /// <returns>The parsed response from the API as a StatesResponse object.</returns>
        private async Task<StatesResponse> QueryAPI(decimal long_min, decimal long_max, decimal lat_min, decimal lat_max)
        {
           
            // Build the URL.
            var url = $"{OPENSKY_API_URL}?lamin={lat_min}&lomin={long_min}&lamax={lat_max}&lomax={long_max}";

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"OpenSky API Returned error code: {response.StatusCode}");
            }
            var content = await response.Content.ReadAsStringAsync();
            // Deserialize the object to fill the Time and States values.
            return JsonConvert.DeserializeObject<StatesResponse>(content);
        }

        /// <summary>
        /// Map the unnamed JSON States objects to a list of State objects.
        /// </summary>
        /// <param name="statesResponse">The StatesResponse object that got deserialized.</param>
        /// <returns>The StatesResponse object with ConvertedStates value filled in if applicable.</returns>
        private StatesResponse MapObjectsToStates(StatesResponse statesResponse)
        {
            // Instantly return if there are no States.
            if (statesResponse.States == null)
            {
                return statesResponse;
            }

            // Convert the states manually and add them to a list to slot into the response object.
            var convertedStates = new List<State>();
            foreach (var state in statesResponse.States)
            {
                // Remove leading [ and trailing ] and split on commas.
                var splitState = state.ToString().Trim('[', ']').Split(',');
                // Trim endings of all splits.
                for (int i = 0; i < splitState.Length; i++)
                {
                    // Remove all \r, \n and " occurences and trim excess whitespace.
                    splitState[i] = splitState[i].Replace("\r", "").Replace("\n", "").Replace('"', ' ').Trim();
                }

                var mappedState = new State()
                {
                    ICAO24 = splitState[0],
                    CallSign = splitState[1],
                    OriginCountry = splitState[2],
                    // Skipped parsing 3 to 16 (except for 14) since they can be null.
                    Squawk = splitState[14]
                };
                if (!splitState[3].Equals("null"))
                {
                    mappedState.TimePosition = int.Parse(splitState[3]);
                }
                if (!splitState[4].Equals("null"))
                {
                    mappedState.LastContact = int.Parse(splitState[4]);
                }
                if (!splitState[5].Equals("null"))
                {
                    mappedState.Longitude = decimal.Parse(splitState[5]);
                }
                if (!splitState[6].Equals("null"))
                {
                    mappedState.Latitude = decimal.Parse(splitState[6]);
                }
                if (!splitState[7].Equals("null"))
                {
                    mappedState.BaroAltitude = decimal.Parse(splitState[7]);
                }
                if (!splitState[8].Equals("null"))
                {
                    mappedState.OnGround = bool.Parse(splitState[8]);
                }
                if (!splitState[9].Equals("null"))
                {
                    mappedState.Velocity = decimal.Parse(splitState[9]);
                }
                if (!splitState[10].Equals("null"))
                {
                    mappedState.TrueTrack = decimal.Parse(splitState[10]);
                }
                if (!splitState[11].Equals("null"))
                {
                    mappedState.VerticalRate = decimal.Parse(splitState[11]);
                }
                // Map the sensors int array from string to an actual int array.
                if (!splitState[12].Equals("null"))
                {
                    var sensorsStringArray = splitState[12].Split(',');
                    var sensors = new List<int>();
                    foreach (var sensorString in sensorsStringArray)
                    {
                        sensors.Add(int.Parse(sensorString));
                    }
                    mappedState.Sensors = sensors.ToArray();
                }
                if (!splitState[13].Equals("null"))
                {
                    mappedState.GeoAltitude = decimal.Parse(splitState[13]);
                }
                if (!splitState[15].Equals("null"))
                {
                    mappedState.Spi = bool.Parse(splitState[15]);
                }
                if (!splitState[16].Equals("null"))
                {
                    mappedState.PositionSource = int.Parse(splitState[16]);
                }

                convertedStates.Add(mappedState);
            }

            statesResponse.ConvertedStates = convertedStates;

            return statesResponse;
        }
    }
}