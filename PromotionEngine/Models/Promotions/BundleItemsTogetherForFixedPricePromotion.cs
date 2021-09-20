using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle different items for a total price
     * 
     * Example:  C & D for 30
     */
    public class BundleItemsTogetherForFixedPricePromotion : MultipleSkuIdsPromotion
    {
        public BundleItemsTogetherForFixedPricePromotion(int priority, List<string> skuIds, float totalPrice): base(priority, skuIds)
        {
            TotalPrice = totalPrice;
        }
        public float TotalPrice { get; }

    }
}
