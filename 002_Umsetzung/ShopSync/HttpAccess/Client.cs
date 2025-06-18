using System.Diagnostics;
using Model;

namespace HttpAccess;

public class Client
{
    private readonly List<Config> configs = [];
    private readonly HttpClient httpClient;

    public Client()
    {
        //ToDo: make configurable
        configs.Add(new Config
        {
            Url = "https://test.webstores.ch/boreas/shop/api/v2",
            Username = "boreas",
            Password = "3bc2ba9843b0490599b9dd163bd88d4f"
        });
        httpClient = new HttpClient();
    }

    /// <summary>
    /// Retrieves a list of all products across multiple configurations (shops).
    /// </summary>
    /// <remarks>This method aggregates products from multiple sources defined by the configurations. Each
    /// product is assigned a shop identifier to ensure multi-tenancy.</remarks>
    /// <returns>A list of <see cref="Product"/> objects representing all products retrieved. The list will be empty if no
    /// products are found.</returns>
    public List<Product> GetProducts()
    {
        List<Product> allProducts = [];

        foreach (Config config in configs)
        {
            List<Product> thisProducts = GetProductsForUrl(config).Result;

            // To ensure multi-tenancy, each product is assigned the shop
            foreach (Product product in thisProducts)
            {
                SetShop(product, config);
            }
            allProducts.AddRange(thisProducts);
        }
        return allProducts;
    }
    
    private async Task<List<Product>> GetProductsForUrl(Config config)
    {
        string productGetUrl = $"{config.Url}/products";
        
        string json = await GetRequest(productGetUrl, config);

        var productResponse = System.Text.Json.JsonSerializer.Deserialize<ProductsResponse>(json, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return productResponse?.Data ?? [];
    }

    //private void PostProduct(Config config, Product )
    //{
    //    string productPostUrl = $"{config.Url}/products";

    //    using (var request = new HttpRequestMessage(HttpMethod.Post, productPostUrl))
    //    {
    //        request.Headers.Authorization = new System.Net.Http.Headers.
    //            AuthenticationHeaderValue("Basic", Convert.ToBase64String(
    //                System.Text.Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}")));
    //        var response = await httpClient.SendAsync(request);
    //        response.EnsureSuccessStatusCode();
    //        json = await response.Content.ReadAsStringAsync();
    //    }

    //}

    private async Task<string> GetRequest(string getUrl, Config config)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, getUrl);

        request.Headers.Authorization = new System.Net.Http.Headers.
            AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}")));

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        return json;
    }

    private static void SetShop(Product product, Config config)
    {
        product.Shop = new Shop { Url = config.Url };
    }
}