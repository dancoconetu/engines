using PromotionEngine.Models;
using PromotionEngine.Models.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;

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


        public static float TryApplyPromotions(List<Item> items, Dictionary<string, float> prices, List<Promotion> applicablePromotions)
        {
            var totalPrice = 0.0f;
            var allSkuIdsFromPromotion = applicablePromotions
                .SelectMany(applicablePromotion => applicablePromotion.GetSkuIds()).Distinct().ToHashSet();

            var itemsAvailableForPromotion = items.Where(item => allSkuIdsFromPromotion.Contains(item.SkuId)).ToList();
            var itemsNotAvailableForPromotion = items.Where(item => !allSkuIdsFromPromotion.Contains(item.SkuId)).ToList();

            totalPrice += CalculateWithPromotions(itemsAvailableForPromotion, prices, applicablePromotions) +
                          CalculateWithoutPromotions(itemsNotAvailableForPromotion, prices);

            return totalPrice;
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
            List<Promotion> applicablePromotions)
        {
            var totalPrice = 0.0f;

            foreach (var promotion in applicablePromotions)
            {
                switch (promotion)
                {
                    case MultipleItemsForFixedPricePromotion multiplePromotion:
                        var multiplePromotionItem = items.FirstOrDefault( item => item.SkuId == multiplePromotion.SkuId);
                        if (multiplePromotionItem == null)
                            throw new ArgumentException($"0 items for promotion with {multiplePromotion.SkuId}");
                        if(!prices.TryGetValue(multiplePromotion.SkuId, out var price))
                            throw new ArgumentException($"Price missing for {multiplePromotion.SkuId}");

                        totalPrice += PromotionCalculatorHelper.GetTotalForMultipleItemsPromotion(multiplePromotionItem, price, multiplePromotion);
                        break;

                    case BundleItemsTogetherForFixedPricePromotion bundlePromotion:
                    {
                        var bundledItems = items.Where(item => bundlePromotion.GetSkuIds().Contains(item.SkuId));
                        if (bundledItems?.Count() == 0)
                            throw new ArgumentException($"0 items for bundle promotion");


                        break;
                    }
                }
            }
            return totalPrice;
        }
    }
}
