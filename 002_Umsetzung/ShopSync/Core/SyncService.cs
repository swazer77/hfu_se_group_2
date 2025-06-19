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
        #region variable for Sync to Api
        public List<Product>? ProductsUpdateToApi { get; set; }
        public List<Product>? ProductsDeleteToApi { get; set; }
        public List<DbProduct>? DbEntitiesErp { get; set; }
        public List<DbProduct>? DbEntitiesDel { get; set; }
        #endregion

        #region variable for Sync to DB
        public List<DbProduct>? AllDbEntities { get; set; }
        public List<DbProduct>? ProductToUpdateInDb { get; set; }
        public List<DbProduct>? ProductToDeleteInDb { get; set; }
        public List<Product>? AllApiProducts { get; set; }
        public List<DbProduct>? AllApiProductsMapped { get; set; }
        #endregion


        #region test data
        private readonly TestApiProducts testApi = new TestApiProducts();
        #endregion

        private readonly DbClient dbClient;
        private readonly Client client;

        public SyncService(DbClient dbClient, Client client)
        {
            this.dbClient = dbClient;
            this.client = client;
        }

        public void Run()
        {
            Init();
            ErrorLog.LogError("Start Synchronisation...");

            #region Step 1
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
            #endregion

            #region Step 2
            //// Step 2
            //// 
            ////
            try
            {
                Console.WriteLine("Get all products from HttpAccess");
                GetProductsFromApi();
                Console.WriteLine("Get all products from Db");
                GetAllDataFromDb();
                Console.WriteLine("Compare DB with API");
                CompareApiWithDb();
                Console.WriteLine("Send update to DB");
                SendProductsToDb();
            }
            catch (Exception e)
            {
                ErrorLog.LogError("Something went wrong while sync the data from the API to the Db.", e);
                
            }
            #endregion

            ErrorLog.LogError("End Synchronisation");
            foreach (string log in ErrorLog.GetErrors())
            {
                Console.WriteLine(log);
            }

        }

        public void Init()
        {
            ProductsUpdateToApi = new List<Product>();
            ProductsDeleteToApi = new List<Product>();
            DbEntitiesDel = new List<DbProduct>();
            DbEntitiesErp = new List<DbProduct>();
            AllDbEntities = new List<DbProduct>();
            ProductToUpdateInDb = new List<DbProduct>();
            ProductToDeleteInDb = new List<DbProduct>();
            AllApiProducts = new List<Product>();
            AllApiProductsMapped = new List<DbProduct>();
        }

        #region Sync Step 1 methodes
        private void GetProductsFromDbWithErpChanged()
        {
            List<DbProduct> dbProducts = dbClient.GetAllProductsErpChanged();

            DbEntitiesDel = dbProducts.Where(p =>
                p.ErpChanged.ToString().Equals("D", StringComparison.OrdinalIgnoreCase)).ToList();
            ProductsDeleteToApi = ProductMapper.ToModel(DbEntitiesDel);

            DbEntitiesErp = dbProducts
                .Where(p => p.ErpChanged.ToString().Equals("U", StringComparison.OrdinalIgnoreCase)).ToList();
            ProductsUpdateToApi = ProductMapper.ToModel(DbEntitiesErp);
        }

        private async Task SendProductsUpdateToApi()
        {
            if (ProductsUpdateToApi == null || ProductsUpdateToApi.Count == 0) return;

            try
            {
                await client.PostProducts(ProductsUpdateToApi);
                foreach (DbProduct p in DbEntitiesErp)
                {
                    dbClient.SetFlagsForProductById(p.Id, 'C', 'C');
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending update product to API.", ex);
                throw;
            }
        }

        private async Task SendProductsDeleteToApi()
        {
            if (ProductsDeleteToApi == null || ProductsDeleteToApi.Count == 0) return;

            try
            {
                await client.DeleteProducts(ProductsDeleteToApi);
                foreach (DbProduct p in DbEntitiesDel)
                {
                    dbClient.SetFlagsForProductById(p.Id, (char)p.ErpChanged, 'D');
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending delete product to API.", ex);
                throw;
            }
        }
        #endregion

        #region Sync Step 2 methodes
        private void GetProductsFromApi()
        {
            try
            {
                AllApiProducts = client.GetProducts();
            }
            catch (Exception e)
            {
                ErrorLog.LogError("Failed to get Products from API.", e);
                throw;
            }
            //TODO: to be removed
            Console.WriteLine($"Found {AllApiProducts.Count} products from API.");
        }

        private void GetAllDataFromDb()
        {
            AllDbEntities = dbClient.GetAllProducts();
        }

        private void CompareApiWithDb()
        {
            if (AllApiProducts == null || AllApiProducts.Count == 0) return;

            AllApiProductsMapped = ProductMapper.ToEntity(AllApiProducts);

            ProductToUpdateInDb = AllApiProductsMapped
                .Where(api =>
                {
                    var db = AllDbEntities.FirstOrDefault(d => d.ProductId == api.ProductId && d.Shop.Url == api.Shop.Url);
                    return db == null || CompareProducts(db, api);
                })
                .ToList();

            ProductToDeleteInDb = AllDbEntities
                .Where(db =>
                    !AllApiProductsMapped.Any(api =>
                        api.ProductId == db.ProductId &&
                        api.Shop.Url == db.Shop.Url))
                .ToList();
        }

        private bool CompareProducts(DbProduct db, DbProduct api)
        {
            return db.ProductId != api.ProductId ||
                   db.Type != api.Type ||
                   db.Shop.Url != api.Shop.Url ||
                   db.Attributes.Created != api.Attributes.Created ||
                   db.Attributes.LastModified != api.Attributes.LastModified ||
                   db.Attributes.Price.GetHashCode() != api.Attributes.Price.GetHashCode() ||
                   db.Attributes.LiveFrom != api.Attributes.LiveFrom ||
                   db.Attributes.LiveUntil != api.Attributes.LiveUntil ||
                   db.Attributes.Locale.First().Name != api.Attributes.Locale.First().Name ||
                   db.Attributes.Locale.First().Language != api.Attributes.Locale.First().Language;
        }

        private void SendProductsToDb()
        {
            if (ProductToUpdateInDb != null && ProductToUpdateInDb.Count != 0)
            {
                ProductToUpdateInDb.ForEach(p => p.ShopChanged = 'U');
                dbClient.InsertOrUpdateProducts(ProductToUpdateInDb);
            }

            if (ProductToDeleteInDb != null && ProductToDeleteInDb.Count != 0)
            {
                ProductToDeleteInDb.ForEach(p => p.ShopChanged = 'D');
                dbClient.InsertOrUpdateProducts(ProductToDeleteInDb);
            }
        }
        #endregion
    }

}
