using DbAccess;
using DBModel;
using HttpAccess;
using HttpModel;
using Moq;

namespace Core.Tests
{
    [TestClass]
    public sealed class SyncServiceTests
    {
        private Mock<DbClient> mockDbClient;
        private Mock<Client> mockApiClient;
        private SyncService syncService;

        [TestInitialize]
        public void TestInitialize()
        {
            mockDbClient = new Mock<DbClient>();
            mockApiClient = new Mock<Client>();
            syncService = new SyncService(mockDbClient.Object, mockApiClient.Object);
            syncService.Init(); // reset internal lists
        }

        [TestMethod]
        public async Task SendProductsUpdateToApi_ShouldCallPostAndSetFlags()
        {
            // Arrange
            var testProduct = new Product { Id = "1", Shop = new Shop { Url = "shop1" } };
            var dbProduct = new DbProduct { Id = 1, ErpChanged = 'U' };

            syncService.ProductsUpdateToApi = new List<Product> { testProduct };
            syncService.DbEntitiesErp = new List<DbProduct> { dbProduct };

            mockApiClient.Setup(c => c.PostProducts(It.IsAny<List<Product>>()))
                .Returns(Task.CompletedTask);

            // Act
            await syncService.SendProductsUpdateToApi();

            // Assert
            mockApiClient.Verify(c => c.PostProducts(It.IsAny<List<Product>>()), Times.Once);
            mockDbClient.Verify(db => db.SetFlagsForProductById(1, 'C', 'C'), Times.Once);
        }

        [TestMethod]
        public void CompareApiWithDb_ShouldIdentifyChanges()
        {
            // Arrange
            var dbProduct = new DbProduct
            {
                ProductId = "1",
                Shop = new DbShop { Url = "shop1" },
                Attributes = new DbAttributes { Price = 10 }
            };

            var apiProduct = new DbProduct
            {
                ProductId = "1",
                Shop = new DbShop { Url = "shop1" },
                Attributes = new DbAttributes { Price = 20 } // Different
            };

            syncService.AllDbEntities = new List<DbProduct> { dbProduct };
            syncService.AllApiProductsMapped = new List<DbProduct> { apiProduct };

            // Act
            syncService.CompareApiWithDb();

            // Assert
            Assert.AreEqual(1, syncService.ProductToUpdateInDb.Count);
            Assert.AreEqual(0, syncService.ProductToDeleteInDb.Count);
        }
    }
}
