using DbAccess;
using DBModel;
using Microsoft.EntityFrameworkCore;

namespace DbAccessTests;

[TestClass]
public sealed class DbClientTests
{
    [TestMethod]
    public void TestInsertProducts()
    {
        List<DbProduct> products =
        [
            new DbProduct
            {
                ProductId = "TestProduct1",
                Type = "TestType",
                Attributes = new DbAttributes
                {
                    Locale = [new DbLocale { Language = "de", Name = "Test Product 1" }],
                    Price = 19.90,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LiveFrom = DateTime.Now,
                    LiveUntil = DateTime.Now
                },
                ErpChanged = null,
                ShopChanged = 'I',
                Shop = new DbShop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            },

            new DbProduct
            {
                ProductId = "TestProduct2",
                Type = "TestType",
                Attributes = new DbAttributes
                {
                    Locale = [new DbLocale { Language = "de", Name = "Test Product 2" }],
                    Price = 29.90,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LiveFrom = DateTime.Now,
                    LiveUntil = DateTime.Now
                },
                ErpChanged = 'U',
                ShopChanged = 'C',
                Shop = new DbShop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            },

            new DbProduct
            {
                ProductId = "TestProduct3",
                Type = "TestType",
                Attributes = new DbAttributes
                {
                    Locale = [new DbLocale { Language = "de", Name = "Test Product 3" }],
                    Price = 99.90,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LiveFrom = DateTime.Now,
                    LiveUntil = DateTime.Now
                },
                ErpChanged = null,
                ShopChanged = 'D',
                Shop = new DbShop
                {
                    Url = "https://test.webstores2.ch/boreas/shop/api/v2"
                }
            }
        ];

        DbClient dbClient = new DbClient();
        Context dbContext = new Context();
        // Ensure the database is created and cleared for testing
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
        dbClient.InsertOrUpdateProducts(products);
        List<DbProduct> dbProducts = dbContext.Product.ToList();
        List<DbShop> dbShops = dbContext.Shop.ToList();
        Assert.IsNotNull(dbProducts, "Products list should not be null.");
        Assert.IsTrue(dbProducts.Count == 3, "Expected at least 3 products in the database.");
        Assert.IsTrue(dbShops.Count == 2, "Duplicate Shops were created");
        Assert.IsTrue(dbProducts.Any(p => p.ProductId == "TestProduct1"), "Inserted product should be in the database.");
        Assert.IsTrue(dbProducts.Any(p => p.ProductId == "TestProduct2"), "Inserted product should be in the database.");
        Assert.IsTrue(dbProducts.Any(p => p.ProductId == "TestProduct3"), "Inserted product should be in the database.");
    }

    [TestMethod]
    public void TestChangeProducts()
    {
        List<DbProduct> initialProducts =
        [
            new DbProduct
            {
                ProductId = "TestProduct4",
                Type = "TestType",
                Attributes = new DbAttributes
                {
                    Locale = [new DbLocale { Language = "de", Name = "Test Product 4" }],
                    Price = 19.90,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LiveFrom = DateTime.Now,
                    LiveUntil = DateTime.Now
                },
                ErpChanged = 'U',
                ShopChanged = null,
                Shop = new DbShop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            }
        ];

        List<DbProduct> changedProducts =
        [
            new DbProduct
            {
                ProductId = "TestProduct4",
                Type = "TestType",
                Attributes = new DbAttributes
                {
                    Locale = [new DbLocale { Language = "de", Name = "Test Product 4" }],
                    Price = 99.90,
                    Created = DateTime.Now,
                    LastModified = DateTime.Now,
                    LiveFrom = DateTime.Now,
                    LiveUntil = DateTime.Now
                },
                ErpChanged = 'U',
                ShopChanged = 'C',
                Shop = new DbShop
                {
                    Url = "https://test.webstores.ch/boreas/shop/api/v2"
                }
            }
        ];

        DbClient dbClient = new DbClient();
        using Context dbContext = new Context();
        // Ensure the database is created and cleared for testing
        dbClient.InsertOrUpdateProducts(initialProducts);
        DbProduct dbProduct = dbContext.Product.Include("Attributes").First(p => p.ProductId == "TestProduct4");
        Assert.AreEqual(19.90, dbProduct.Attributes.Price, "Initial insert failed.");
        Assert.AreEqual(dbProduct.ShopChanged, null, "Initial insert failed.");
        dbClient.InsertOrUpdateProducts(changedProducts);
        dbProduct = dbContext.Product.Include("Attributes").First(p => p.ProductId == "TestProduct4");
        Assert.AreEqual(99.90, dbProduct.Attributes.Price, "Price should have been updated.");
        Assert.AreEqual('C', dbProduct.ShopChanged, "ShopChanged should have been modified.");
    }

    [TestMethod]
    public void TestGetAllProducts()
    {
        DbClient dbClient = new DbClient();
        List<DbProduct> products = dbClient.GetAllProducts();
        Assert.IsNotNull(products);
        Assert.IsTrue(products.Count > 0, "Expected at least one product in the database.");
    }

    [TestMethod]
    public void TestGetAllProductsErpChanged()
    {
        DbClient dbClient = new DbClient();
        List<DbProduct> products = dbClient.GetAllProductsErpChanged();
        Assert.IsNotNull(products);
        Assert.IsTrue(products.Count > 0, "Expected at least one product in the database.");
    }
}