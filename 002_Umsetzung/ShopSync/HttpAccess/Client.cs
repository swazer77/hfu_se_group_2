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

    public async Task<List<Product>> GetProductsForUrl(Config config)
    {
        string productUrl = $"{config.Url}/products";
        string json;

        using (var request = new HttpRequestMessage(HttpMethod.Get, productUrl))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}")));
            var response = await httpClient.SendAsync(request);
            // throw Exception if status is not OK
            response.EnsureSuccessStatusCode();
            json = await response.Content.ReadAsStringAsync();
        }

        var productResponse = System.Text.Json.JsonSerializer.Deserialize<ProductResponse>(json, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return productResponse?.Data ?? [];
    }

    public List<Product> GetProducts()
    {
        List<Product> allProducts = [];
        
        foreach (Config config in configs)
        {
            List<Product> thisProducts = GetProductsForUrl(config).Result;
            allProducts.AddRange(thisProducts);
        }
        return allProducts;
    }
}