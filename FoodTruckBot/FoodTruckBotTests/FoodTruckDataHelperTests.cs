using FoodTruckBot.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace FoodTruckBotTests
{
    [TestClass]
    public class FoodTruckDataHelperTests
    {
        [TestMethod]
        public void LoadFoodTruckData_Should_Succeed()
        {
            var results = FoodTruckDataHelper.LoadFoodTruckData();
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count(), 488);

            var record = results.First();
            Assert.IsNotNull(record.Coordinate);
            Assert.AreEqual(record.Coordinate.Latitude, record.Latitude);
            Assert.AreEqual(record.Coordinate.Longitude, record.Longitude);
        }

        [TestMethod]
        public void GetClosestTrucks_Should_Succeed()
        {
            var trucks = FoodTruckDataHelper.LoadFoodTruckData();

            var results = FoodTruckDataHelper.FindFiveClosetTrucks(trucks, 37.7620192, -122.4273064);
            Assert.AreEqual(results.Count(), 5);
            var top = results.First();
            var expectedTruck = new FoodTruck { 
                Applicant = "The Geez Freeze",
                Address = "3750 18TH ST",
                Latitude = 37.7620192,
                Longitude = -122.4273064
            };

            Assert.AreEqual(top.Applicant, expectedTruck.Applicant);
            Assert.AreEqual(top.Address, expectedTruck.Address);
            Assert.AreEqual(top.Latitude, expectedTruck.Latitude);
            Assert.AreEqual(top.Longitude, expectedTruck.Longitude);
        }
    }
}
