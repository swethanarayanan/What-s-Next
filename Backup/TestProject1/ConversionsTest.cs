using ToDoList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ConversionsTest and is intended
    ///to contain all ConversionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConversionsTest
    {


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
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ConvertArrayOfStringIntoListOfInt
        ///</summary>
        [TestMethod()]
        public void ConvertArrayOfStringIntoListOfIntTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string[] s = {"1","2","3"}; // TODO: Initialize to an appropriate value
            List<int> actual;
            actual = target.ConvertArrayOfStringIntoListOfInt(s);
            Debug.Assert(actual != null);
        }

        /// <summary>
        ///A test for ConvertDaysToHours
        ///</summary>
        [TestMethod()]
        public void ConvertDaysToHoursTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string days = "2"; // TODO: Initialize to an appropriate value
            double expected = 48F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.ConvertDaysToHours(days);
            Assert.AreEqual(expected, actual);
           // Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertMinutesToHours
        ///</summary>
        [TestMethod()]
        public void ConvertMinutesToHoursTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string minutes = "60"; // TODO: Initialize to an appropriate value
            double expected = 1F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.ConvertMinutesToHours(minutes);
            Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertSecondsToHours
        ///</summary>
        [TestMethod()]
        public void ConvertSecondsToHoursTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string seconds = "3600"; // TODO: Initialize to an appropriate value
            double expected = 1F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.ConvertSecondsToHours(seconds);
            Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertStringToDate
        ///</summary>
        [TestMethod()]
        public void ConvertStringToDateTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string s = "12/07/2007"; // TODO: Initialize to an appropriate value
            DateTime actual;
            actual = target.ConvertStringToDate(s);
            Debug.Assert(actual != null);
           // Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ConvertDateTo_ddmmyyFormat
        ///</summary>
        [TestMethod()]
        public void ConvertDateTo_ddmmyyFormatTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string _date = "12-07-2010"; // TODO: Initialize to an appropriate value
            string expected = "07/12/2010"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.ConvertDateTo_ddmmyyFormat(_date);
            Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Convert24HourFormatTo12HourFormat
        ///</summary>
        [TestMethod()]
        public void Convert24HourFormatTo12HourFormatTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            TimeSpan s = TimeSpan.MaxValue; // TODO: Initialize to an appropriate value
           //string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.Convert24HourFormatTo12HourFormat(s);
            Debug.Assert(actual != null);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Convert12HourFormatTo24HourFormat
        ///</summary>
        [TestMethod()]
        public void Convert12HourFormatTo24HourFormatTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string s = "1 pm"; // TODO: Initialize to an appropriate value
          //  TimeSpan expected = new TimeSpan(); // TODO: Initialize to an appropriate value
            TimeSpan actual;
            actual = target.Convert12HourFormatTo24HourFormat(s);
            Debug.Assert(actual != null);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        ///// <summary>
        /////A test for convertEventsIntoTaskdetails
        /////</summary>
        //[TestMethod()]
        //public void convertEventsIntoTaskdetailsTest()
        //{
        //    Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
        //    Event events = new Event("1", "dinner", "12/08/2010 4pm", "12/08/2010 6pm", "high", "2 hr"); // TODO: Initialize to an appropriate value
        //    TaskDetails TaskDetailsObj = new TaskDetails();
        //    TaskDetailsObj.setID("1");
        //    TaskDetailsObj.setEventname("dinner");
        //    TaskDetailsObj.setlocation(null);
        //    TaskDetailsObj.setPriority("high");
        //    TaskDetailsObj.setReminder("1H");
        //    TaskDetailsObj.setStartDate("12/08/2010");
        //    TaskDetailsObj.setStartTime("4 pm");
        //    TaskDetailsObj.setEndTime("6 pm");
        //    TaskDetailsObj.setEndDate("12/08/2010");
        //    TaskDetailsObj.setDuration("2H");
        //    TaskDetailsObj.setEndRepeat(null);
        //    TaskDetailsObj.setRepeat("none");
        //    TaskDetails expected = TaskDetailsObj; // TODO: Initialize to an appropriate value
        //    TaskDetails actual;
        //    actual = target.convertEventsIntoTaskdetails(events);
        //    Assert.AreEqual(expected, actual);
        //   // Assert.Inconclusive("Verify the correctness of this test method.");
        //}

        /// <summary>
        ///A test for convertStringIntoTaskDetails
        ///</summary>
        [TestMethod()]
        public void convertStringIntoTaskDetailsTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            List<string> ListOfCompletedEvents = null; // TODO: Initialize to an appropriate value
            List<TaskDetails> expected = null; // TODO: Initialize to an appropriate value
            List<TaskDetails> actual;
            actual = target.convertStringIntoTaskDetails(ListOfCompletedEvents);
            Assert.AreEqual(expected, actual);
         //   Assert.Inconclusive("Verify the correctness of this test method.");
        }

     
        /// <summary>
        ///A test for durationOrRemainderToTotalHours
        ///</summary>
        [TestMethod()]
        public void durationOrRemainderToTotalHoursTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string s = "2 hr";// TODO: Initialize to an appropriate value
          //  TimeSpan expected = new TimeSpan(); // TODO: Initialize to an appropriate value
            TimeSpan actual;
            actual = target.durationOrRemainderToTotalHours(s);
            Debug.Assert(actual != null);
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for interpretRepeatAsNumberOfDays
        ///</summary>
        [TestMethod()]
        public void interpretRepeatAsNumberOfDaysTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string s = string.Empty; // TODO: Initialize to an appropriate value
            string date = string.Empty; // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.interpretRepeatAsNumberOfDays(s, date);
            Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for replaceWithAppropriateShortForms
        ///</summary>
        [TestMethod()]
        public void replaceWithAppropriateShortFormsTest()
        {
            Conversions target = new Conversions(); // TODO: Initialize to an appropriate value
            string s = string.Empty; // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.replaceWithAppropriateShortForms(s);
            Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("Verify the correctness of this test method.");
        }

        
    }
}
