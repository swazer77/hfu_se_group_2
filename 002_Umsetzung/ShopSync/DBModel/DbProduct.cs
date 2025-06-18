namespace DBModel;

public class DbProduct
{
    public int Id { get; set; }
    public string? ProductId { get; set; }
    public string? Type { get; set; }
    public DbShop? Shop { get; set; }
}