namespace DBModel;

public class DbLocale
{
    public int Id { get; set; }
    public string Language { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DbAttributesId { get; set; }
}