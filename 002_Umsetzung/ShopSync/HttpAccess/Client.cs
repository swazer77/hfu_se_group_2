using System.Text.Json;
using HttpAccess.Wrappers;
using Model;

namespace HttpAccess;

public class Client
{
    private readonly List<Config> configs = [];
    private readonly HttpClient httpClient = new();

    private readonly JsonSerializerOptions jsonSerializerOptions = new(new JsonSerializerOptions())
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public Client()
    {
        //ToDo: make configurable
        configs.Add(new Config
        {
            Url = "https://test.webstores.ch/boreas/shop/api/v2",
            Username = "boreas",
            Password = "3bc2ba9843b0490599b9dd163bd88d4f"
        });
    }

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

    public async Task PostProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            await PostProduct(product);
        }
    }

    private async Task<List<Product>> GetProductsForUrl(Config config)
    {
        string productGetUrl = $"{config.Url}/products";
        
        string json = await GetRequest(productGetUrl, config);

        ProductsWrapper? productResponse = JsonSerializer.Deserialize<ProductsWrapper>(json, jsonSerializerOptions);

        return productResponse?.Data ?? [];
    }

    private async Task PostProduct(Product product)
    {
        Config config = configs.Find(c => c.Url == product.Shop?.Url)
                        ?? throw new InvalidDataException("Could not find matching shop configuration.");

        string productPostUrl = $"{config.Url}/products?update_if_exists=true";

        ProductWrapper postProduct = new() { Data = product };

        string json = JsonSerializer.Serialize(postProduct, jsonSerializerOptions);

        await PostRequest(productPostUrl, config, json);
    }

    private async Task<string> GetRequest(string getUrl, Config config)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, getUrl);

        request.Headers.Authorization = GetAuthorizationHeader(config);

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    private async Task PostRequest(string postUrl, Config config, string postData)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, postUrl);

        request.Content = new StringContent(postData, System.Text.Encoding.UTF8, "application/json");

        request.Headers.Authorization = GetAuthorizationHeader(config);

        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private static System.Net.Http.Headers.AuthenticationHeaderValue GetAuthorizationHeader(Config config)
    {
        string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}"));
        return new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    private static void SetShop(Product product, Config config)
    {
        product.Shop = new Shop { Url = config.Url };
    }
}