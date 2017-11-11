using ToDoList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for OperationTest and is intended
    ///to contain all OperationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OperationTest
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
        ///A test for Operation Constructor
        ///</summary>
        [TestMethod()]
        public void OperationConstructorTest()
        {
            string scommand = "add"; // TODO: Initialize to an appropriate value
            string scontent = "\"birthday party\" at \"soc\" from \"6 pm\""; // TODO: Initialize to an appropriate value
            Operation target = new Operation(scommand, scontent);
            Debug.Assert(target != null);
           // Assert.Inconclusive("TODO: Implement code to verify target");
        }



    

        


      




      

        /// <summary>
        ///A test for creatDateEventObject
        ///</summary>
        [TestMethod()]
        public void creatDateEventObjectTest()
        {
            string scommand = string.Empty; // TODO: Initialize to an appropriate value
            string scontent = string.Empty; // TODO: Initialize to an appropriate value
            Operation target = new Operation(scommand, scontent); // TODO: Initialize to an appropriate value
            TimeSpan time = new TimeSpan(); // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
           
            DateEvent actual;
            actual = target.creatDateEventObject(time, name);
            Debug.Assert(actual != null);
        
        }

        /// <summary>
        ///A test for decideOntask
        ///</summary>
        [TestMethod()]
        public void decideOntaskTest()
        {
            string scommand = string.Empty; // TODO: Initialize to an appropriate value
            string scontent = string.Empty; // TODO: Initialize to an appropriate value
            Operation target = new Operation(scommand, scontent); // TODO: Initialize to an appropriate value
            List<TaskDetails> expected = null; // TODO: Initialize to an appropriate value
            List<TaskDetails> actual;
            actual = target.decideOntask();
            Assert.AreEqual(expected, actual);
           
        }


        /// <summary>
        ///A test for getDisplayMessage
        ///</summary>
        [TestMethod()]
        public void getDisplayMessageTest()
        {
            string scommand = string.Empty; // TODO: Initialize to an appropriate value
            string scontent = string.Empty; // TODO: Initialize to an appropriate value
            Operation target = new Operation(scommand, scontent); // TODO: Initialize to an appropriate value
            string actual;
            actual = target.getDisplayMessage();
            Assert.AreEqual(null, actual);
        //    Assert.Inconclusive("Verify the correctness of this test method.");
        }

  

        
        
    }
}
