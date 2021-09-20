using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to bundle different items for a total price
     * 
     * Example:  C & D for 30
     */
    public class BundleItemsTogetherForFixedPricePromotion : Promotion
    {
        public BundleItemsTogetherForFixedPricePromotion(int priority, List<string> skuIds, float totalPrice): base(priority)
        {
            SkuIds = skuIds;
            TotalPrice = totalPrice;
        }
        public List<string> SkuIds { get; }
        public float TotalPrice { get; }

    }
}
