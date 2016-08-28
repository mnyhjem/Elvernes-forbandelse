using System.Collections.Generic;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ElvenCurse.Tests.Core
{
    [TestClass]
    public class ExtensionsAndUtilityTests
    {
        [TestMethod]
        public void IsWithinReachOf_Works()
        {
            var validFromlocations = new List<Location>();
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 1, Y = 2});
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 2, Y = 2});
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 3, Y = 2});

            validFromlocations.Add(new Location {WorldsectionId = 1, X = 1, Y = 3});
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 2, Y = 3});//<-- target
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 3, Y = 3});

            validFromlocations.Add(new Location {WorldsectionId = 1, X = 1, Y = 4});
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 2, Y = 4});
            validFromlocations.Add(new Location {WorldsectionId = 1, X = 3, Y = 4});

            var destination = new Location
            {
                WorldsectionId = 1,
                X = 2,
                Y = 3
            };

            for (var y = 0; y <= 5; y++)
            {
                for (var x = 0; x <= 5; x++)
                {
                    var fromLocation = new Location {WorldsectionId = 1, X = x, Y = y};
                    var result = fromLocation.IsWithinReachOf(destination, 1);

                    var isValid = validFromlocations.Count(a => a.X == fromLocation.X && a.Y == fromLocation.Y) == 1;
                    Assert.AreEqual(isValid, result, string.Format("X {0} Y {1} failed. Result where {2}. Should have been {3}", fromLocation.X, fromLocation.Y, result, isValid));
                }
            }
        }
    }
}
