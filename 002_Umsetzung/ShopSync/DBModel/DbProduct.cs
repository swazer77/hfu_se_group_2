namespace DBModel;

public class DbProduct
{
    public int Id { get; set; }
    public string ProductId { get; set; } = null!;
    public DbAttributes Attributes { get; set; } = null!;
    public string Type { get; set; } = null!;
    public DbShop Shop { get; set; } = null!;
    public char? ErpChanged { get; set; }
    public char? ShopChanged { get; set; }
}