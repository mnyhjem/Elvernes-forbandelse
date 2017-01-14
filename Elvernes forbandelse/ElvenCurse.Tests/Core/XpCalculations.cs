using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElvenCurse.Tests.Core
{
    /// <summary>
    /// Summary description for XpCalculations
    /// </summary>
    [TestClass]
    public class XpCalculations
    {
        public XpCalculations()
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
        public void XpRequiredToAdvanceToNextLevel_Works_For_All_90_Levels()
        {

            Assert.AreEqual(400, ElvenCurse.Core.Utilities.ExperienceCalculations.XpRequiredToAdvanceToNextLevel(1), 0, "Level 1 not working");
        }

        [TestMethod]
        public void XpEarnedOnCurrentLevel_Works()
        {
            var xpTotal = 1591;
            var expectedResult = 291;

            Assert.AreEqual(expectedResult, ElvenCurse.Core.Utilities.ExperienceCalculations.XpEarnedOnCurrentLevel(3, xpTotal));
        }

        [TestMethod]
        public void CurrentLevelFromAccXp_Works()
        {
            var xpTotal = 1591;
            var expectedResult = 3;

            Assert.AreEqual(expectedResult, ElvenCurse.Core.Utilities.ExperienceCalculations.CurrentlevelFromAccumulatedXp(xpTotal));
        }

        [TestMethod]
        public void CurrentLevelFromAccXp_Works_At_0_Xp()
        {
            var xpTotal = 0;
            var expectedResult = 1;

            Assert.AreEqual(expectedResult, ElvenCurse.Core.Utilities.ExperienceCalculations.CurrentlevelFromAccumulatedXp(xpTotal));
        }
    }
}
