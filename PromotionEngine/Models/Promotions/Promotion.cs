using System;
using System.Collections.Generic;

namespace PromotionEngine.Models.Promotions
{
    public abstract class Promotion
    {
        protected Promotion(int priority)
        {
            
            Priority = priority;
        }
        
        public int Priority { get;  }


        public virtual List<string> GetSkuIds()
        {
            throw new NotImplementedException();
        }
    }
}
