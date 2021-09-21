using System;
using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{
    public interface IPromotion
    {
       
        int Priority { get; }

        List<string> SkuIds { get; }

        float CalculateTotal(List<Item> items, Dictionary<string, float> prices);
    }
}
