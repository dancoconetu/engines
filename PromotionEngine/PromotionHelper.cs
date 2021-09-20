using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;

namespace PromotionEngine
{
    public static class PromotionHelper
    {
        public static List<Promotion> GetApplicablePromotions(List<Item> items, List<Promotion> promotions)
        {
            if (promotions.Select(p => p.Priority).Distinct().Count() != promotions.Count)
                throw new ArgumentException("Promotions have same priority id");
            
            var usedSkuIds = new HashSet<string>();
            var orderedPromotions = promotions.OrderBy(p => p.Priority);
            var applicablePromotions = new List<Promotion>();
            foreach (var promotion in orderedPromotions)
            {
                switch (promotion)
                {
                    case BundleItemsTogetherForFixedPricePromotion bundleItemsPromotion:
                    {
                        var skuIds = bundleItemsPromotion.SkuIds.ToHashSet();
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

                        break;
                    }
                    case MultipleItemsForFixedPricePromotion multipleItemsPromotion:
                    {
                        var skuId = multipleItemsPromotion.SkuId;
                        if (usedSkuIds.Contains(skuId))
                            break;

                        if (items.Find(item => item.SkuId == skuId) != null)
                        {
                            usedSkuIds.Add(skuId);
                            applicablePromotions.Add(promotion);
                        }

                        break;

                    }
                    case PercentageDiscountPromotion percentageDiscountPromotion:
                    {
                        var skuId = percentageDiscountPromotion.SkuId;
                        if (usedSkuIds.Contains(skuId)) break;

                        if (items.Find(item => item.SkuId == skuId) != null)
                        {
                            usedSkuIds.Add(skuId);
                            applicablePromotions.Add(promotion);
                        }

                        break;

                    }

                }
            }

            return applicablePromotions;
        }


        public static float ApplyPromotions(List<Item> items, Dictionary<string, float> prices, List<Promotion> promotions)
        {
            var totalPrice = 0.0f;
            foreach (var item in items)
            {
                prices.TryGetValue(item.SkuId, out var price);
                totalPrice += price * item.Quantity;
            }

            return totalPrice;
        }
    }
}
