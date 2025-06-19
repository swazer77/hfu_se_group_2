using System.Diagnostics;
using System.Text.Json;
using HttpAccess.Wrappers;
using HttpModel;
using Utility;

namespace HttpAccess;

public class Client
{
    private readonly List<ShopConfig> configs;
    private readonly HttpClient httpClient = new();

    private readonly JsonSerializerOptions jsonSerializerOptions = new(new JsonSerializerOptions())
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public Client()
    {
        configs = new ConfigReader().GetShopConfig();
        if (configs.Count == 0)
        {
            throw new InvalidDataException("No shop configurations found. Please check your config file.");
        }
    }


    // ############################################################################################
    // Public methods
    // ############################################################################################

    // Product ####################################################################################

    public List<Product> GetProducts()
    {
        List<Product> allProducts = [];

        foreach (ShopConfig config in configs)
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

    public async Task DeleteProducts(List<Product> products)
    {
        foreach (Product product in products)
        {
            await DeleteProduct(product);
        }
    }


    // ############################################################################################
    // Private methods
    // ############################################################################################

    // Product ####################################################################################

    private async Task<List<Product>> GetProductsForUrl(ShopConfig config)
    {
        string productGetUrl = $"{config.Url}/products";
        string json = await GetRequest(productGetUrl, config);
        ProductsWrapper? productResponse = JsonSerializer.Deserialize<ProductsWrapper>(json, jsonSerializerOptions);
        return productResponse?.Data ?? [];
    }

    private async Task PostProduct(Product product)
    {
        ShopConfig config = GetConfig(product);
        string productPostUrl = $"{config.Url}/products?update_if_exists=true";
        ProductWrapper postProduct = new() { Data = product };
        string json = JsonSerializer.Serialize(postProduct, jsonSerializerOptions);
        await PostRequest(productPostUrl, config, json);
    }

    private async Task DeleteProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Id))
        {
            throw new InvalidDataException("Product ID cannot be null or empty for deletion.");
        }

        ShopConfig config = GetConfig(product);
        string deleteUrl = $"{config.Url}/products/{product.Id}";
        await DeleteRequest(deleteUrl, config);
    }


    // ############################################################################################
    // Basic HTTP methods for GET, POST, and DELETE requests
    // ############################################################################################

    private async Task<string> GetRequest(string getUrl, ShopConfig config)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, getUrl);
        request.Headers.Authorization = GetAuthorizationHeader(config);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task PostRequest(string postUrl, ShopConfig config, string postData)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, postUrl);
        request.Content = new StringContent(postData, System.Text.Encoding.UTF8, "application/json");
        request.Headers.Authorization = GetAuthorizationHeader(config);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private async Task DeleteRequest(string deleteUrl, ShopConfig config)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, deleteUrl);
        request.Headers.Authorization = GetAuthorizationHeader(config);
        HttpResponseMessage response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }


    // ############################################################################################
    // Helper methods
    // ############################################################################################

    private static System.Net.Http.Headers.AuthenticationHeaderValue GetAuthorizationHeader(ShopConfig config)
    {
        string credentials = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{config.Username}:{config.Password}"));
        return new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
    }

    private static void SetShop(Product product, ShopConfig config)
    {
        product.Shop = new Shop { Url = config.Url };
    }

    private ShopConfig GetConfig(object category)
    {
        ShopConfig? config = null;

        if (category is Product product)
        {
            config = configs.Find(c => c.Url == product.Shop?.Url);
        }

        return config ?? throw new InvalidDataException("Could not find matching shop configuration.");
    }
}