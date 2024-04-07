using Models;

namespace BusinessLogic.Rules.SelectionRules
{
    public class CustomerCardValidationRule : IValidationRule
    {
        public (bool isValid, string errorMessage) Validate(ValidationContext context)
        {
            CustomerCard customerCard = context.CustomerCard;
            int animalCount = context.SelectedAnimals.Count;
            bool hasVIPAnimal = context.SelectedAnimals.Any(a => a.AnimalType.TypeName == "VIP");

            if (customerCard == null)
            {
                if (animalCount > 3)
                {
                    return (false, "Klanten zonder klantenkaart mogen maximaal 3 dieren boeken.");
                }
            }
            else if (customerCard.CardType.Equals("Silver"))
            {
                if (animalCount > 4)
                {
                    return (false, "Klanten met een zilveren klantenkaart mogen maximaal 4 dieren boeken.");
                }
            }

            if (hasVIPAnimal && (customerCard == null || !customerCard.CardType.Equals("Platinum")))
            {
                return (false, "Alleen klanten met een platina klantenkaart kunnen VIP dieren boeken.");
            }

            return (true, null);
        }
    }
}
