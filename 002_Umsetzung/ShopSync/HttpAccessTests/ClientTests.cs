using System.Diagnostics;
using HttpAccess;

namespace HttpAccessTests;

/// <summary>
/// Tests for class <see cref="Client"/>.
/// The shop API is not mocked, so a real API access as described in the
/// project description is required.
/// </summary>
[TestClass]
public sealed class ClientTests
{
    private Client? client;

    [TestInitialize]
    public void TestInitialize()
    {
        client = new Client();
    }


    [TestMethod]
    public void TestGetProducts()
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