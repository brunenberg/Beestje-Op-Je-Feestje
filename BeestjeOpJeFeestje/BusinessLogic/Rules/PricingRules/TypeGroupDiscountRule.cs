using Models;

namespace BusinessLogic.Rules.PricingRules
{
    public class TypeGroupDiscountRule : IDiscountRule
    {
        public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context)
        {
            IEnumerable<IGrouping<int, Animal>> animalGroups = context.SelectedAnimals.GroupBy(a => a.AnimalTypeId);
            if (animalGroups.Any(a => a.Count() >= 3))
            {
                return (10, new List<string> { "3 beestjes van hetzelfde type: 10%" });
            }
            return (0, null);
        }
    }
}