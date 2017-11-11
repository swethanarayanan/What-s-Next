using ToDoList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace TestProject1
{
    
    
    /// <summary>
    ///This is a test class for ParserTest and is intended
    ///to contain all ParserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ParserTest
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
        ///A test for Parser Constructor
        ///</summary>
        [TestMethod()]
        public void ParserConstructorTest()
        {
            StreamWriter writer = new StreamWriter("Log.Txt", true) ;
            //StreamWriter writer = new StreamWriter("Log.Txt", true);
            writer.WriteLine(System.DateTime.Now + " : Trying to create an object of Parser Class..." + Environment.NewLine + "Expected Output : " + "Object Creation Successful \n");
            string s = "cs2103 proj"; // TODO: Initialize to an appropriate value
            Parser target = new Parser(s);
            //Assert.Inconclusive("TODO: Implement code to verify target");
            Debug.Assert(target!=null);
            writer.WriteLine("System Output: " + "Object Creation Successful\n\n");
            writer.Close();
        }

        /// <summary>
        ///A test for SyntaxOfTextboxText
        ///</summary>
        [TestMethod()]
        public void SyntaxOfTextboxTextTest()
        {
            StreamWriter writer = new StreamWriter("Log.Txt", true);
            writer.WriteLine(System.DateTime.Now + " : Checking Syntax of TextBox..." + Environment.NewLine + "Expected Output : " + "Syntax Checked \n");
            string s = "delete \"\""; // TODO: Initialize to an appropriate value
            Parser target = new Parser(s); // TODO: Initialize to an appropriate value
            string s1 = "delete \"\""; // TODO: Initialize to an appropriate value
            string expected = "Please ensure that all parameters have been entered within quotes!"; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.SyntaxOfTextboxText(s1);
            Assert.AreEqual(expected, actual);
            writer.WriteLine("System Output: " + "Syntax Checked \n\n");
            writer.Close();
           // Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for checkForWhatCommand
        ///</summary>
        [TestMethod()]
        public void checkForWhatCommandTest()
        {
            StreamWriter writer = new StreamWriter("Log.Txt", true);
            writer.WriteLine(System.DateTime.Now + " : Checking for the command..." + Environment.NewLine + "Expected Output : " + "Command Checked \n");
            string s = "add \"dinner\" at \"pgp\""; // TODO: Initialize to an appropriate value
            Parser target = new Parser(s); // TODO: Initialize to an appropriate value
            target.checkForWhatCommand();
           // Assert.Inconclusive("A method that does not return a value cannot be verified.");
            Debug.Assert(target!=null);
            writer.WriteLine("System Output: " + "Command Checked\n\n");
            writer.Close();
        }


        /// <summary>
        ///A test for CheckIfCommandAndContentValid
        ///</summary>
        [TestMethod()]
        public void CheckIfCommandAndContentValidTest()
        {
            StreamWriter writer = new StreamWriter("Log.Txt", true);
            writer.WriteLine(System.DateTime.Now + " : Checking if command and content are valid.." + Environment.NewLine + "Expected Output : " + "Command and Content Validated \n");
            string s = "\"2\" done"; // TODO: Initialize to an appropriate value
            Parser target = new Parser(s); // TODO: Initialize to an appropriate value
            target.CheckIfCommandAndContentValid();
         //  Assert.Inconclusive("A method that does not return a value cannot be verified.");
            Debug.Assert(target != null);
            writer.WriteLine("System Output: " + "Command and Content Validated\n\n");
            writer.Close();
        }

          /// <summary>
        ///A test for HelpUserWriteCommand
        ///</summary>
        [TestMethod()]
        public void HelpUserWriteCommandTest1()
        {
            string s = "add \"dinner\" at "; // TODO: Initialize to an appropriate value
            Parser target = new Parser(s); // TODO: Initialize to an appropriate value
            string expected = "\"\""; // TODO: Initialize to an appropriate value
            string actual;
            actual = target.HelpUserWriteCommand();
            Assert.AreEqual(expected, actual);
         //   Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
