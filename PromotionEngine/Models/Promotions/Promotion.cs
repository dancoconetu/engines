namespace PromotionEngine.Models.Promotions
{
    public abstract class Promotion
    {
        protected Promotion(int priority)
        {
            
            Priority = priority;
        }
        
        public int Priority { get;  }
    }
}
