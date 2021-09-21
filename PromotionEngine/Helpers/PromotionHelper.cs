using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromotionEngine.Helpers
{
    public static class PromotionHelper
    {
        public static float TryApplyPromotions(List<Item> items, Dictionary<string, float> prices, List<IPromotion> promotions)
        {
            var applicablePromotions = GetApplicablePromotions(items,promotions);
            var totalPrice = 0.0f;
            var allSkuIdsFromPromotion = applicablePromotions
                .SelectMany(applicablePromotion => applicablePromotion.SkuIds).Distinct().ToHashSet();

            var itemsAvailableForPromotion = items.Where(item => allSkuIdsFromPromotion.Contains(item.SkuId)).ToList();
            var itemsNotAvailableForPromotion = items.Where(item => !allSkuIdsFromPromotion.Contains(item.SkuId)).ToList();

            totalPrice += CalculateWithPromotions(itemsAvailableForPromotion, prices, applicablePromotions) +
                          CalculateWithoutPromotions(itemsNotAvailableForPromotion, prices);

            return totalPrice;
        }

        public static List<IPromotion> GetApplicablePromotions(List<Item> items, List<IPromotion> promotions)
        {
            if (promotions.Select(p => p.Priority).Distinct().Count() != promotions.Count)
                throw new ArgumentException("Promotions have same priority id");

            var usedSkuIds = new HashSet<string>();
            var orderedPromotions = promotions.OrderBy(p => p.Priority);
            var applicablePromotions = new List<IPromotion>();
            foreach (var promotion in orderedPromotions)
            {
                var skuIds = promotion.SkuIds.ToHashSet();
                if (usedSkuIds.Overlaps(skuIds))
                    break;
                var hashSet = new HashSet<string>();
                var applyThisPromotion = false;
                foreach (var item in items)
                {
                    if (skuIds.Contains(item.SkuId))
                    {
                        hashSet.Add(item.SkuId);
                    }

                    if (hashSet.Count == skuIds.Count)
                    {
                        applyThisPromotion = true;
                        break;
                    }
                }
                if (applyThisPromotion)
                {
                    usedSkuIds.UnionWith(hashSet);
                    applicablePromotions.Add(promotion);
                }

            }

            return applicablePromotions;
        }


        public static float CalculateWithoutPromotions(List<Item> items, Dictionary<string, float> prices)
        {
            var totalPrice = 0.0f;
            foreach (var item in items)
            {
                prices.TryGetValue(item.SkuId, out var price);
                totalPrice += price * item.Quantity;
            }

            return totalPrice;

        }

        public static float CalculateWithPromotions(List<Item> items, Dictionary<string, float> prices,
            List<IPromotion> applicablePromotions)
        {
            var totalPrice = 0.0f;

            foreach (var promotion in applicablePromotions)
            {
                totalPrice += promotion.CalculateTotal(items, prices);
            }
            return totalPrice;
        }
    }
}
