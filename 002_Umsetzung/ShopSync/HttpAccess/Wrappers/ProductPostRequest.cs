using System.Text.Json.Serialization;

namespace Model;

public class ProductPostRequest
{
    [JsonPropertyName("update_if_exists")]
    public bool UpdateIfExists { get; set; }
    public Product? Data { get; set; }
}