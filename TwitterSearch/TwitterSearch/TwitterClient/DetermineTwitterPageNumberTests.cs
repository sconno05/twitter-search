using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaginationConverterTests
{
    /// <summary>
    /// A sample unit test.  I extracted it from a separate unit test project that I created for developing the pagination logic.
    /// It isn't executable in this app because I wrote it at home with Visual Web Developer, which does not support unit testing.
    /// </summary>
    [TestClass]
    public class DetermineTwitterPageNumberTests
    {
        public DetermineTwitterPageNumberTests()
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

        #region Client results per page = 10

        [TestMethod]
        public void Page1_LowerBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(1, 10, 100);
            Assert.AreEqual(1, twitterPageNumber);
        }

        [TestMethod]
        public void Page1_UpperBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(10, 10, 100);
            Assert.AreEqual(1, twitterPageNumber);
        }

        [TestMethod]
        public void Page2_LowerBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(11, 10, 100);
            Assert.AreEqual(2, twitterPageNumber);
        }

        [TestMethod]
        public void Page2_UpperBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(20, 10, 100);
            Assert.AreEqual(2, twitterPageNumber);
        }

        #endregion

        #region Client results per page = 5

        [TestMethod]
        public void FiveResultsPerClientPage_Page1_LowerBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(1, 5, 100);
            Assert.AreEqual(1, twitterPageNumber);
        }

        [TestMethod]
        public void FiveResultsPerClientPage_Page1_UpperBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(20, 5, 100);
            Assert.AreEqual(1, twitterPageNumber);
        }

        [TestMethod]
        public void FiveResultsPerClientPage_Page2_LowerBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(21, 5, 100);
            Assert.AreEqual(2, twitterPageNumber);
        }

        [TestMethod]
        public void FiveResultsPerClientPage_Page2_UpperBound()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(40, 5, 100);
            Assert.AreEqual(2, twitterPageNumber);
        }

        #endregion

        #region Parameter validation

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClientPageIsZero()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(0, 10, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClientResultsPerPageIsZero()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(10, 0, 100);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TwitterResultsPerPageIsZero()
        {
            int twitterPageNumber = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(10, 10, 0);
        }

        #endregion

        #region Client results per page equals twitter results per page

        [TestMethod]
        public void ClientResultsPerPageEqualsTwitterResultsPerPage_Page1()
        {
            int twitterPage = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(1, 10, 10);
            Assert.AreEqual(1, twitterPage);
        }

        [TestMethod]
        public void ClientResultsPerPageEqualsTwitterResultsPerPage_Page2()
        {
            int twitterPage = PaginationConverter.TwitterSearchPager.DetermineTwitterPageNumber(2, 10, 10);
            Assert.AreEqual(2, twitterPage);
        }

        #endregion
    }
}
