using Core.io;
using Core.mapper;
using Core.testdata;
using DbAccess;
using DBModel;
using HttpAccess;
using HttpModel;

namespace Core
{
    public class SyncService
    {
        public List<Product>? ProductsUpdateToApi { get; set; }
        public List<Product>? ProductsDeleteToApi { get; set; }

        public List<DbProduct>? AllDbEntities { get; set; }
        public List<DbProduct>? DbEntitiesErp { get; set; }
        public List<DbProduct>? DbEntitiesDel { get; set; }

        private readonly TestApiProducts testApi = new TestApiProducts();

        public readonly DbClient DbClient = new DbClient();
        public readonly Client Client = new Client();

        public void Run()
        {
            //// Step 1
            //// Sync from DB to Api
            //// Get all updated Data form the DB, map it and send depended on the flag to the delete or update endpoint
            try
            {
                Console.WriteLine("Get products from DB with ERP flag !'C'");
                GetProductsFromDbWithErpChanged();

                Console.WriteLine("Send update to API");
                SendProductsUpdateToApi().Wait();

                Console.WriteLine("Send deletion to API");
                SendProductsDeleteToApi().Wait();
            }
            catch (Exception e)
            {
                ErrorLog.LogError("Something went wrong while sync the data from the DB and sending to the API.", e);
            }
            


            //// Step 2
            //// 
            ////
            Console.WriteLine("Get products from HttpAccess");
            GetProductsFromApi();

            

            Console.WriteLine("Get products from HttpAccess");
            GetProductsFromApi();

            foreach (string log in ErrorLog.GetErrors())
            {
                Console.WriteLine(log);
            }
        }

        public void GetProductsFromDbWithErpChanged()
        {
            List<DbProduct> dbProducts = DbClient.GetAllProductsErpChanged();

            DbEntitiesDel = dbProducts.Where(p =>
                p.ErpChanged.ToString().Equals("D", StringComparison.OrdinalIgnoreCase)).ToList();
            ProductsDeleteToApi = ProductMapper.ToModel(DbEntitiesDel);

            DbEntitiesErp = dbProducts
                .Where(p => p.ErpChanged.ToString().Equals("U", StringComparison.OrdinalIgnoreCase)).ToList();
            ProductsUpdateToApi = ProductMapper.ToModel(DbEntitiesErp);
        }

        public void GetProductsFromApi()
        {
            
            List<Product> allProducts = Client.GetProducts();
            Console.WriteLine($"Found {allProducts.Count} products from API.");
        }

        public async Task SendProductsUpdateToApi()
        {
            if (ProductsUpdateToApi == null || ProductsUpdateToApi.Count == 0) return;

            try
            {
                await Client.PostProducts(ProductsUpdateToApi);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending update product to API.", ex);
            }
        }

        public async Task SendProductsDeleteToApi()
        {
            if (ProductsDeleteToApi == null || ProductsDeleteToApi.Count == 0) return;

            try
            {
                await Client.DeleteProducts(ProductsDeleteToApi);
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending delete product to API.", ex);
            }
        }
    }

}
