using BusinessLogic.Interfaces;
using Models;

namespace BusinessLogic.RuleGroups
{
    public class SelectionRules : ISelectionRules
    {
        private readonly List<IValidationRule> _selectionRules;

        public SelectionRules(List<IValidationRule> selectionRules) {
            _selectionRules = selectionRules;
        }

        public (bool isValid, string errorMessage) ValidateAnimals(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate)
        {
            ValidationContext context = new ValidationContext
            {
                SelectedAnimals = selectedAnimals,
                CustomerCard = customerCard,
                BookingDate = bookingDate
            };

            foreach (IValidationRule rule in _selectionRules)
            {
                (bool isValid, string errorMessage) result = rule.Validate(context);
                if (!result.isValid) return result;
            }

            return (true, null);
        }
    }
}
