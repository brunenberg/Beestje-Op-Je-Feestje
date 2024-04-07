
namespace BusinessLogic.Rules.PricingRules {
    public class MaxDiscountRule : IDiscountRule {
        public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context) {
            if (context.DiscountPercentage > 60) {
                return (60, null);
            }
            return (context.DiscountPercentage, null);
        }
    }
}
