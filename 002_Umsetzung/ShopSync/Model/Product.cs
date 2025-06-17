namespace Model
{
    public class Product
    {
        public int Id { get; set; }
        public string? ProductId { get; set; }
        public char? ErpChanged { get; set; }
        public Attribute? Attribute { get; set; }
        public char? ShopChanged { get; set; }
        public Shop? Shop { get; set; }
    }
}
