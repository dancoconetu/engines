namespace PromotionEngine.Models
{
    
    public class Item
    {
        public Item(string skuId, int quantity)
        {
            SkuId = skuId;
            Quantity = quantity;
        }
        public string SkuId { get;  }
        public int Quantity { get; }
    }
}
