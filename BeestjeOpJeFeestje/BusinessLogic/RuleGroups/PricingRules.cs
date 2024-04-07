using BusinessLogic.Interfaces;
using BusinessLogic.Rules.PricingRules;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace BusinessLogic.RuleGroups
{
    public class PricingRules : IPricingRules
    {
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

            List<IDiscountRule> discountRules = new List<IDiscountRule>
            {
                new TypeGroupDiscountRule(),
                new DuckDiscountRule(new ConcreteRandomNumberGenerator()),
                new DayDiscountRule(),
                new NameDiscountRule(),
                new CustomerCardDiscountRule(),
        };

            
            List<string> appliedDiscounts = new List<string>();

            foreach (IDiscountRule rule in discountRules) {
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
