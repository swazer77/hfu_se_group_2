namespace DBModel;

public class DbAttributes
{
    public int Id { get; set; }
    public List<DbLocale> Locale { get; set; } = null!;
    public DateTime? LastModified { get; set; }
    public DateTime? Created { get; set; }
    public double Price { get; set; }
    public DateTime LiveFrom { get; set; }
    public DateTime LiveUntil { get; set; }
}