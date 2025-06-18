using DbAccess;
using DBModel;

namespace DbAccessTests;

[TestClass]
public sealed class DbClientTests
{
    [TestMethod]
    public void TestInsertProduct()
    {
        DbProduct product = new DbProduct
        {
            ProductId = "TestProduct1235",
            Type = "TestType",
            Attributes = new()
            {
                Locale = [new DbLocale { Language = "de", Name = "Test Product Group 2" }],
                Price = 15,
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                LiveFrom = DateTime.Now,
                LiveUntil = DateTime.Now
            },
            ErpChanged = 'C',
            ShopChanged = 'C',
            Shop = new DbShop
            {
                Url = "https://test.webstores.ch/boreas/shop/api/v2"
            }
        };

        DbClient dbClient = new DbClient();
        dbClient.InsertProduct(product);
    }

    [TestMethod]
    public void TestGetAllProducts()
    {
        DbClient dbClient = new DbClient();
        List<DbProduct> products = dbClient.GetAllProducts();
        Assert.IsNotNull(products);
        //Assert.IsTrue(products.Count > 0, "Expected at least one product in the database.");
    }
}