namespace Model
{
    public class Product
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public Attribute? Attribute { get; set; }
        public Shop? Shop { get; set; }
    }
}
