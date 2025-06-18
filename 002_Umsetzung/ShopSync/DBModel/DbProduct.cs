namespace DBModel;

public class DbProduct
{
    public int Id { get; set; }
    public string? ProductId { get; set; }
    public DbAttributes? Attributes { get; set; }
    public string? Type { get; set; }
    public DbShop? Shop { get; set; }
    public char? ErpChanged { get; set; }
    public char? ShopChanged { get; set; }
}