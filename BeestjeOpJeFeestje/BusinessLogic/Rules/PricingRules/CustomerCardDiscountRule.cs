using Models;

namespace BusinessLogic.Rules.PricingRules
{
    public class CustomerCardDiscountRule : IDiscountRule
    {
        public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context)
        {
            if (context.CustomerCard != null) {
                return (10, new List<string> { "Korting voor klantenkaart: 10%" });
            }
            return (0, null);
        }
    }
}