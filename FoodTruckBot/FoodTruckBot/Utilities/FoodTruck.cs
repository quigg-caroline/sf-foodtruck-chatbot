using CsvHelper.Configuration.Attributes;
using Geolocation;

namespace FoodTruckBot.Utilities
{
    // Class representing a food truck from dataset
    public class FoodTruck
    {
        public string Applicant { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Ignore]
        public Coordinate Coordinate => new Coordinate(this.Latitude, this.Longitude);
    }
}
