using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{
    public abstract class OneSkuIdPromotion: Promotion
    {
        protected OneSkuIdPromotion(int priority, string skuId) : base(priority)
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
