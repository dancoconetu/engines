using System.Collections.Generic;

namespace PromotionEngine.Repositories
{
    public class PriceRepository: IPriceRepository
    {
        public Dictionary<string, float> GetItemPrices()
        {
            var prices = new Dictionary<string, float>
            {
                {"A", 50},
                {"B", 30},
                {"C", 20},
                {"D", 15}
            };

            return prices;
        }
    }
}
