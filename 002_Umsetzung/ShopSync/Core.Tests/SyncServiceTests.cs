using DbAccess;
using DBModel;
using HttpAccess;
using HttpModel;
using Moq;

namespace Core.Tests
{
    [TestClass]
    public class SyncServiceTests
    {
        private Mock<DbClient> _mockDbClient;
        private Mock<Client> _mockApiClient;
        private SyncService _syncService;

        [TestInitialize]
        public void Setup()
        {
            _mockDbClient = new Mock<DbClient>();
            _mockApiClient = new Mock<Client>();

            _syncService = new SyncService(_mockDbClient.Object, _mockApiClient.Object);
            _syncService.Init(); // initialize internal lists
        }

        [TestMethod]
        public async Task SendProductsUpdateToApi_ShouldPostAndUpdateFlags()
        {
            // Arrange
            var apiProduct = new Product { Id = "1", Shop = new Shop { Url = "shop1" } };
            var dbProduct = new DbProduct { ProductId = "1", ErpChanged = 'U' };

            _syncService.ProductsUpdateToApi = new List<Product> { apiProduct };
            _syncService.DbEntitiesErp = new List<DbProduct> { dbProduct };

            _mockApiClient.Setup(c => c.PostProducts(It.IsAny<List<Product>>()))
                .Returns(Task.CompletedTask);

            // Act
            await InvokePrivateMethodAsync(_syncService, "SendProductsUpdateToApi");

            // Assert
            _mockApiClient.Verify(c => c.PostProducts(It.IsAny<List<Product>>()), Times.Once);
            _mockDbClient.Verify(db => db.SetFlagsForProductById(1, 'C', 'C'), Times.Once);
        }

        [TestMethod]
        public void CompareApiWithDb_ShouldDetectUpdatesAndDeletions()
        {
            // Arrange
            var dbProduct = new DbProduct
            {
                ProductId = "1",
                Shop = new DbShop { Url = "shop1" },
                Type = "A",
                Attributes = new DbAttributes
                {
                    Price = 10,
                    Created = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    LiveFrom = DateTime.UtcNow,
                    LiveUntil = DateTime.UtcNow,
                    Locale = new List<DbLocale> { new DbLocale { Name = "EN", Language = "EN" } }
                }
            };

            var apiProduct = new DbProduct
            {
                ProductId = "1",
                Shop = new DbShop { Url = "shop1" },
                Type = "A",
                Attributes = new DbAttributes
                {
                    Price = 20, // difference
                    Created = dbProduct.Attributes.Created,
                    LastModified = dbProduct.Attributes.LastModified,
                    LiveFrom = dbProduct.Attributes.LiveFrom,
                    LiveUntil = dbProduct.Attributes.LiveUntil,
                    Locale = new List<DbLocale> { new DbLocale { Name = "EN", Language = "EN" } }
                }
            };

            _syncService.AllDbEntities = new List<DbProduct> { dbProduct };
            _syncService.AllApiProductsMapped = new List<DbProduct> { apiProduct };
            _syncService.AllApiProducts = new List<Product> { new Product() }; // to pass null check

            // Act
            InvokePrivateMethod(_syncService, "CompareApiWithDb");

            // Assert
            Assert.AreEqual(1, _syncService.ProductToUpdateInDb.Count);
            Assert.AreEqual(0, _syncService.ProductToDeleteInDb.Count);
        }

        // Helpers to invoke private methods
        private async Task InvokePrivateMethodAsync(object obj, string methodName)
        {
            var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method == null) throw new InvalidOperationException($"Method '{methodName}' not found.");
            var task = (Task)method.Invoke(obj, null)!;
            await task;
        }

        private void InvokePrivateMethod(object obj, string methodName)
        {
            var method = obj.GetType().GetMethod(methodName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (method == null) throw new InvalidOperationException($"Method '{methodName}' not found.");
            method.Invoke(obj, null);
        }
    }
}
