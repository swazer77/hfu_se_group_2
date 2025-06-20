using Core.io;

namespace Core.Tests
{
    [TestClass]
    public sealed class ErrorLogTests
    {


        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestMethod]
        public void WriteToErrorLog()
        {
            //Arrange
            string error = "this is a test run";
            string exString = "Index out of bounds";
            Exception ex = new IndexOutOfRangeException(exString);
            
            //Act
            Logger.LogError(error, ex);

            //Asset
            List<string> logs = Logger.GetLogs();
            string firstLine = logs.FirstOrDefault() ?? string.Empty;

            Assert.IsTrue(firstLine.Contains(error), "Error message was not logged correctly.");
            Assert.IsTrue(logs.Any(line => line.Contains(exString)), "Exception message was not logged.");

        }


        [TestMethod]
        public void GetStringArrayFromErrorLog()
        {
            // Arrange
            string testMessage = "Test message from GetLogs test";
            Exception testException = new InvalidOperationException("Test exception for GetLogs");
            Logger.LogError(testMessage, testException);

            //Act
            List<string> logLines = Logger.GetLogs();

            //Asset
            Assert.IsNotNull(logLines, "Log lines should not be null.");
            Assert.IsTrue(logLines.Count > 0, "Log should contain at least one line.");
            Assert.IsTrue(logLines.Any(line => line.Contains(testMessage)), "Log should contain the test message.");
            Assert.IsTrue(logLines.Any(line => line.Contains(testException.Message)), "Log should contain the exception message.");

        }
    }
}
