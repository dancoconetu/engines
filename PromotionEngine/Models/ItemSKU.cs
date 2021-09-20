namespace PromotionEngine.Models
{
    public class ItemSKU
    {
        public ItemSKU(string id, float price)
        {
            Id = id;
            Price = price;
        }

        public string Id { get;}
        public float Price { get;}
    }
}
