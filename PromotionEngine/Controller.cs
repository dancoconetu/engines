using System.Collections.Generic;
using PromotionEngine.Models;
using PromotionEngine.Repositories;

namespace PromotionEngine
{
    public class Controller
    {
        private readonly IPromotionRepository _promotionRepository;
        public Controller(IPromotionRepository promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
        public float CalculateTotal(List<Item> items)
        {
            return 100;
        }


        

    }
}
