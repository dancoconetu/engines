using System.Collections.Generic;


namespace PromotionEngine.Models.Promotions
{
    public class MultipleSkuIdsPromotion: Promotion

    {
        public MultipleSkuIdsPromotion(int priority, List<string> skuIds) : base(priority)
        {
            SkuIds = skuIds;
        }
        public List<string> SkuIds { get; }

        public override List<string> GetSkuIds()
        {
            return SkuIds;
        }
    }
}
