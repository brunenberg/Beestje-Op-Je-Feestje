using BusinessLogic.Interfaces;
using BusinessLogic.Rules.SelectionRules;
using Models;

namespace BusinessLogic.RuleGroups
{
    public class SelectionRules : ISelectionRules
    {

        public (bool isValid, string errorMessage) ValidateAnimals(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate)
        {
            ValidationContext context = new ValidationContext
            {
                SelectedAnimals = selectedAnimals,
                CustomerCard = customerCard,
                BookingDate = bookingDate
            };

            List<IValidationRule> validationRules = new List<IValidationRule>
            {
                new AnimalCountValidationRule(),
                new AnimalTypeValidationRule(),
                new BookingDayValidationRule(),
                new BookingMonthValidationRule(),
                new CustomerCardValidationRule()
            };

            foreach (IValidationRule rule in validationRules)
            {
                (bool isValid, string errorMessage) result = rule.Validate(context);
                if (!result.isValid) return result;
            }

            return (true, null);
        }
    }
}
