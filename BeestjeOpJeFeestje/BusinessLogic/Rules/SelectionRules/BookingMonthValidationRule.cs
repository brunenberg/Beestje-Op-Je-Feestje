namespace BusinessLogic.Rules.SelectionRules
{
    public class BookingMonthValidationRule : IValidationRule
    {
        public (bool isValid, string errorMessage) Validate(ValidationContext context)
        {
            if (context.SelectedAnimals.Any(a => a.AnimalType.TypeName == "Woestijn" && (context.BookingDate.Month >= 10 || context.BookingDate.Month <= 2)))
            {
                return (false, "Je mag geen beestje boeken van het type 'Woestijn' in de maanden oktober t/m februari.");
            }

            if (context.SelectedAnimals.Any(a => a.AnimalType.TypeName == "Sneeuw" && context.BookingDate.Month >= 6 && context.BookingDate.Month <= 8))
            {
                return (false, "Je mag geen beestje boeken van het type 'Sneeuw' in de maanden juni t/m augustus.");
            }
            return (true, null);
        }
    }
}

