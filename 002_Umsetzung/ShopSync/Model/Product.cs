namespace HttpModel
{
    public class Product
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public Attributes? Attributes { get; set; }
        public Shop? Shop { get; set; }
    }
}
