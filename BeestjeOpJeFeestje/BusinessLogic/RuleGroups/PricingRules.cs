using BusinessLogic.Interfaces;
using BusinessLogic.Rules.PricingRules;
using Models;

namespace BusinessLogic.RuleGroups
{
    public class PricingRules : IPricingRules
    {
        private readonly List<IDiscountRule> _discountRules;

        public PricingRules(List<IDiscountRule> discountRules) {
            _discountRules = discountRules;
        }

        public double CalculateAnimalsPrice(List<Animal> animals)
        {
            double totalPrice = 0;
            foreach (Animal animal in animals)
            {
                totalPrice += animal.Price;
            }
            return totalPrice;
        }

        public (int discountPercentage, List<string> errorMessages) CalculateDiscount(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate)
        {
            int discountPercentage = 0;

            DiscountContext context = new DiscountContext {
                SelectedAnimals = selectedAnimals,
                CustomerCard = customerCard,
                BookingDate = bookingDate,
                DiscountPercentage = discountPercentage
            };
            
            List<string> appliedDiscounts = new List<string>();

            foreach (IDiscountRule rule in _discountRules) {
                (int discountPercentage, List<string> discountMessages) result = rule.GetDiscount(context);
                context.DiscountPercentage += result.discountPercentage;
                if (result.discountMessages != null) {
                    appliedDiscounts.AddRange(result.discountMessages);
                }
            }

            IDiscountRule maxDiscountRule = new MaxDiscountRule();
            context.DiscountPercentage = maxDiscountRule.GetDiscount(context).discountPercentage;

            return (context.DiscountPercentage, appliedDiscounts);
        }
    }
}
