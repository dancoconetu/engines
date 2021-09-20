using PromotionEngine.Models;
using System.Collections.Generic;

namespace PromotionEngine.Repositories
{
    public interface IPromotionRepository
    {
        List<Promotion> GetActivePromotions();
    }
}
