using System;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Helpers;
using PromotionEngine.Models;
using PromotionEngine.Repositories;

namespace PromotionEngine
{
    public class Controller
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPriceRepository _priceRepository;
        public Controller(IPromotionRepository promotionRepository, IPriceRepository priceRepository)
        {
            _promotionRepository = promotionRepository;
            _priceRepository = priceRepository;
        }
        public float CalculateTotal(List<Item> items)
        {
            var itemPrices = _priceRepository.GetItemPrices();
            var pricesSkuIds = itemPrices.Select(s => s.Key).ToHashSet();
            var itemsSkuIds = items.Select(i => i.SkuId).ToHashSet();
            if (!pricesSkuIds.IsSupersetOf(itemsSkuIds))
                throw new ArgumentException("Some prices are missing for some sku ids");
            var promotions = _promotionRepository.GetActivePromotions();

            var totalPrice = PromotionHelper.TryApplyPromotions(items, itemPrices, promotions);
            return totalPrice;
        }


        

    }
}
