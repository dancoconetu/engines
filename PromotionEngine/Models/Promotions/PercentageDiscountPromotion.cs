namespace PromotionEngine.Models.Promotions
{

    /*
     * Purpose of the class is to get a percentage for an sku id
     * returns discount percentage for an sku id
     * Example: A has 10% discount
    */
    public class PercentageDiscountPromotion: Promotion
    {
        public PercentageDiscountPromotion(int priority, string skuId, float discount): base(priority)
        {
            Discount = discount;
            SkuId = skuId;
        }

        public string SkuId { get; }

        public float Discount { get; }
    }
}
