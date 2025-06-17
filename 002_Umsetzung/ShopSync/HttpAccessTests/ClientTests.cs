using HttpAccess;

namespace HttpAccessTests;

[TestClass]
public sealed class ClientTests
{
    private Client? client = null;

    [TestInitialize]
    public void TestInitialize()
    {
        client = new Client();
    }


    [TestMethod]
    public void TestMethod1()
    {
        var result = client?.GetProducts();
        Assert.IsNotNull(result, "Result should not be null");
    }
}