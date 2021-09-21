using System.Collections.Generic;

namespace PromotionEngine.Repositories
{
    public interface IPriceRepository
    {
        Dictionary<string, float> GetItemPrices();
    }
}
