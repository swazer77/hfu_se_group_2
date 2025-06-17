using System.Text.Json.Serialization;

namespace Model;

public class Attributes
{
    public List<Locale>? Locale { get; set; }
    [JsonPropertyName("last_modified")]

    public string? LastModified { get; set; }
    public string? Created { get; set; }
    public float Price { get; set; }
}