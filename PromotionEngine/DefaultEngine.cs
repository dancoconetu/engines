using PromotionEngine.Models;
using PromotionEngine.Repositories;
using System.Collections.Generic;

namespace PromotionEngine
{
    public class DefaultEngine
    {
        private readonly IPromotionRepository _promotionRepository;
        public DefaultEngine(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
        public float CalculateTotal(List<Item> items)
        {
            return 100;
        }

    }
}
