namespace FoodTruckBot.Utilities
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CsvHelper;
    using Geolocation;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// Helper class for handling food truck data
    /// </summary>
    public static class FoodTruckDataHelper
    {
        /// <summary>
        /// Load food truck data
        /// </summary>
        /// <returns>Collection of food truck records</returns>
        public static IEnumerable<FoodTruck> LoadFoodTruckData()
        {
            using (var reader = new StreamReader(GetResourceStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<FoodTruck>();
                return records.ToList();
            }
        }

        /// <summary>
        /// Filter food truck list to find five closest to a given location
        /// </summary>
        /// <param name="foodTrucks">Collection of food trucks</param>
        /// <param name="latitude">Input latitude</param>
        /// <param name="longitude">Input longitude</param>
        /// <returns>Collection of five closest food trucks</returns>
        public static IEnumerable<FoodTruck> FindFiveClosetTrucks(IEnumerable<FoodTruck> foodTrucks,double latitude, double longitude)
        {
            var targetCoord = new Coordinate(latitude, longitude);
            var nearest = foodTrucks.OrderBy(truck => GeoCalculator.GetDistance(truck.Coordinate, targetCoord, decimalPlaces: 2));

            return nearest.Take(5);
        }

        /// <summary>
        /// Construct card object based on provided food trucks
        /// </summary>
        /// <param name="trucks">Collection of food truck data</param>
        /// <param name="latitude">Input latitude</param>
        /// <param name="longitude">Input longitude</param>
        /// <returns></returns>
        public static HeroCard GetHeroCard(IEnumerable<FoodTruck> trucks, string latitude, string longitude)
        {
            var cardActions = trucks.Select(x => new CardAction(ActionTypes.OpenUrl, x.Address, value: GetGoogleMapsDirectionUri(x, latitude, longitude))).ToList();
            var heroCard = new HeroCard
            {
                Title = "Food Truck Recommendations",
                Subtitle = "Click to get directions from your location",
                Buttons = cardActions
            };

            return heroCard;
        }

        /// <summary>
        /// Format url to get directions to truck from target location
        /// </summary>
        /// <param name="foodTruck">Food truck data</param>
        /// <param name="latitude">Origin latitude</param>
        /// <param name="longitude">Origin longitude</param>
        /// <returns></returns>
        private static string GetGoogleMapsDirectionUri(FoodTruck foodTruck, string latitude, string longitude)
        {
            var baseUrl = "https://www.google.com/maps/dir/?api=1";
            var query = $"&origin={latitude}%2C{longitude}&destination={foodTruck.Latitude}%2C{foodTruck.Longitude}";
            return baseUrl + query;
        }

        /// <summary>
        /// Get resource stream
        /// </summary>
        /// <returns>Stream for embedded resource</returns>
        private static Stream GetResourceStream()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "FoodTruckBot.Resources.Mobile_Food_Facility_Permit.csv";
            var resourceStream = assembly.GetManifestResourceStream(resourceName);

            return resourceStream;
        }
    }
}
