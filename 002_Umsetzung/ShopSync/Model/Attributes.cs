using System.Text.Json.Serialization;

namespace Model;

public class Attributes
{
    public List<Locale>? Locale { get; set; }
    [JsonPropertyName("last_modified")] public string? LastModified { get; set; }
    public string? Created { get; set; }
    public double Price { get; set; }
    [JsonPropertyName("live_from")] public string? LiveFrom { get; set; }
    [JsonPropertyName("live_until")] public string? LiveUntil { get; set; }
}