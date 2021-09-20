using System.Collections.Generic;
using PromotionEngine.Models.Promotions;

namespace PromotionEngine.Repositories
{
    public interface IPromotionRepository
    {
        List<Promotion> GetActivePromotions();
    }
}
