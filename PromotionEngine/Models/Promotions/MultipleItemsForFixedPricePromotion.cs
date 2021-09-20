
namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle multiple items of the same type/skuId for a total price
     * QuantityNeeded * SkuId for TotalPrice
     * Example: 3 * A for 130
     */
    public class MultipleItemsForFixedPricePromotion: OneSkuIdPromotion
    {
        public MultipleItemsForFixedPricePromotion(int priority, string skuId, int quantityNeeded, float totalPrice): base(priority, skuId)
        {
            QuantityNeeded = quantityNeeded;
            TotalPrice = totalPrice;
        }
        public int QuantityNeeded { get; }
        public float TotalPrice { get; }
    }
}
