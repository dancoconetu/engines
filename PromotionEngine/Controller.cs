using PromotionEngine.Models;
using PromotionEngine.Repositories;
using System.Collections.Generic;
using System.Linq;
using PromotionEngine.Models.Promotions;

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
