using Core.io;
using HttpAccess;
using Model;

namespace Core
{
    public class Program
    {

        public static List<Product>? ShopProducts;

        static void Main(string[] args)
        {
            // Get products from DB where ERP flag is not set 'C'
            Console.WriteLine("Get products from DB with ERP flag 'C'");
            //ShopProducts = GetProductsFromDbWithErpChanged();

            ErrorLog.LogError("no dba object yet");
            // Send error from DB to ErrorLog

            // Send products to API
            Console.WriteLine("Send update to API");
            SendProductsUpdateToApi(ShopProducts);

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


        private static List<Product> GetProductsFromDbWithErpChanged()
        {
            // This method should contain the logic to retrieve products from the database
            // where the ERP flag is not set to 'C'.
            // For now, returning an empty list as a placeholder.
            return new List<Product>();
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
            }
        }

        private static async Task SendProductsUpdateToApi(List<Product>? products)
        {
            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No products to send to API.");
                return;
            }

            Client client = new Client();

            try
            {
                await client.PostProducts(products);
                Console.WriteLine("Products successfully sent to API.");
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Failed to send products to API.", ex);
            }
        }
    }
}
