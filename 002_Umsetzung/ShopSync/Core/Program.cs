using Core.io;
using Core.testdata;
using HttpAccess;
using Model;

namespace Core
{
    public class Program
    {

        public static List<Product>? ProductsUpdateToApi;
        public static List<Product>? ProductsDeleteToApi;

        //Todo: to be removed
        public static TestApiProducts TestApi = new TestApiProducts();

        static void Main(string[] args)
        {
            // Get products from DB where ERP flag is not set 'C'
            Console.WriteLine("Get products from DB with ERP flag 'C'");
            GetProductsFromDbWithErpChanged();

            ErrorLog.LogError("no dba object yet");
            // Send error from DB to ErrorLog

            // Send products update to API
            //Console.WriteLine("Send update to API");

            ////
            Console.WriteLine("Get products from HttpAccess");
            GetProductsFromApi();


            //ProductsUpdateToApi = TestApi.GetCreateProducts();
            //SendProductsUpdateToApi();
            //ProductsUpdateToApi = TestApi.GetUpdateProduct();
            //SendProductsUpdateToApi();

            // Send products deletion to API
            
            Console.WriteLine("Send deletion to API");

            ProductsDeleteToApi = TestApi.GetDeleteProduct();
            SendProductsDeleteToApi();
            

            // Get products from API
            Console.WriteLine("Get products from HttpAccess");
            GetProductsFromApi();
            // Send Error from API to ErrorLog

            // Compare products from DB and API
            Console.WriteLine("");
            // set time stamp from last run into config file
            // when running again, only products with newer timestamp will be compared against DB


            //Todo: to be removed, only for testing purpose
            //Read log file
            foreach (string log in ErrorLog.GetErrors())
            {
                Console.WriteLine(log);
            }
        }


        private static void GetProductsFromDbWithErpChanged()
        {
            // List<ProductEntity> dbProducts = DBAccess.GetAllProductsErpChanged();

            List<Product> products = new List<Product>();

            /*
             * foreach (ProductEntity e in dbProducts)
             * {
             *
             *
             *
             *
             */

            // fill update/Create list


            // fill delete list

        }

        private static void GetProductsFromApi()
        {
            Client client = new Client();
            List<Product> allProducts = client.GetProducts();

            //Todo: to be removed, only for testing purposes
            Console.WriteLine($"Found {allProducts.Count} products from API.");


            foreach (var product in allProducts)
            {
                // Set the shop_fl for each product
                //use mapper
            }
        }

        private static async void SendProductsUpdateToApi()
        {
            if (ProductsUpdateToApi == null || ProductsUpdateToApi.Count == 0)
            {
                //Console.WriteLine("No products to send to API.");
                return;
            }

            Client client = new Client();

            try
            {
                await client.PostProducts(ProductsUpdateToApi);
                //Console.WriteLine("Products successfully sent to API.");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending update product to API.", ex);
            }
        }

        private static async void SendProductsDeleteToApi()
        {
            if (ProductsDeleteToApi == null || ProductsDeleteToApi.Count == 0) return;

            Client client = new Client();

            try
            {
                await client.DeleteProducts(ProductsDeleteToApi);
                //Console.WriteLine("Products successfully sent to API.");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed during sending delete product to API.", ex);
            }
        }
    }
}
