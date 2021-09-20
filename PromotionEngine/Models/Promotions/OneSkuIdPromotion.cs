using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{
    public class OneSkuIdPromotion: Promotion
    {
        public OneSkuIdPromotion(int priority, string skuId) : base(priority)
        {
            SkuId = skuId;
        }
        public string SkuId { get;  }  
        public override List<string> GetSkuIds()
        {
            return new List<string> {SkuId};
        }
    }
}
