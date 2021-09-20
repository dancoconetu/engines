
namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle multiple items of the same type/skuId for a total price
     * QuantityNeeded * SkuId for TotalPrice
     * Example: 3 * A for 130
     */
    public class MultipleItemsForFixedPricePromotion: Promotion
    {
        public MultipleItemsForFixedPricePromotion(int priority, string skuId, int quantityNeeded, float totalPrice): base(priority)
        {
            SkuId = skuId;
            QuantityNeeded = quantityNeeded;
            TotalPrice = totalPrice;
        }
        public string SkuId { get; }
        public int QuantityNeeded { get; }
        public float TotalPrice { get; }
    }
}
