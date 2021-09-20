using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to get a percentage for an sku id
     * returns discount percentage for an sku id
     * Example: A has 10% discount
    */
    public class PercentageDiscountPromotion: OneSkuIdPromotion
    {
        public PercentageDiscountPromotion(int priority, string skuId, float discount): base(priority, skuId)
        {
            Discount = discount;
        }


        public float Discount { get; }

        
    }
}
