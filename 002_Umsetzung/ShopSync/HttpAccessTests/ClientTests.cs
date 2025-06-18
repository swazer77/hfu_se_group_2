using System.Diagnostics;
using HttpAccess;
using Model;

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
            Debug.WriteLine($"LiveFrom: {res.Attributes?.LiveFrom}");
            Debug.WriteLine($"LiveTo: {res.Attributes?.LiveUntil}");
        }
        Assert.IsNotNull(result, "Result should not be null");
    }

    [TestMethod]
    public void TestPostProducts()
    {
        List<Product> postProducts = [
            new()
            {
                Id = "test-product-group2",
                Type = "product",
                Attributes = new Attributes
                {
                    Locale = [new Locale { Language = "de", Name = "Test Product Group 2" }],
                    Price = 99.90,
                    Created = "2025-01-01 00:00:00",
                    LastModified = "",
                    LiveFrom = "2025-01-01 00:00:00",
                    LiveUntil = "2025-12-31 23:59:59"
                },
                Shop = new Shop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2",
                }
            }
        ];

        client?.PostProducts(postProducts).GetAwaiter().GetResult();

        // Additional assertions can be added here to verify the post operation
    }
}