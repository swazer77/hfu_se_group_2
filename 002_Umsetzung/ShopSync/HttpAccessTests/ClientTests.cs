using System.Diagnostics;
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
        foreach (var res in result ?? [])
        {
            Debug.WriteLine($"Id: {res.Id}");
            Debug.WriteLine($"Type: {res.Type}");
            Debug.WriteLine($"Name: {res.Attributes?.Locale?[0].Name}");
            Debug.WriteLine($"LastModified: {res.Attributes?.LastModified}");
            Debug.WriteLine($"Created: {res.Attributes?.Created}");
            Debug.WriteLine($"Price: {res.Attributes?.Price}");
            Debug.WriteLine($"Shop: {res.Shop?.Url}\n");
        }
        Assert.IsNotNull(result, "Result should not be null");
    }
}