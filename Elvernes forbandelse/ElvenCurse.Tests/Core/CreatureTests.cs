using System;
using System.Text;
using System.Collections.Generic;
using ElvenCurse.Core.Model;
using ElvenCurse.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElvenCurse.Tests.Core
{
    /// <summary>
    /// Summary description for CreatureTests
    /// </summary>
    [TestClass]
    public class CreatureTests
    {
        public CreatureTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void DistanceBetweenLocationsAreCalculatedCorrectly()
        {
            var loc1 = new Location { Name = "", WorldsectionId = 1, X = 40, Y = 30 };
            var loc2 = new Location { Name = "", WorldsectionId = 1, X = 45, Y = 30 };

            var res = loc1.GetDistanceBetweenLocations(loc2);
            Assert.AreEqual(5, res);
        }
    }
}
