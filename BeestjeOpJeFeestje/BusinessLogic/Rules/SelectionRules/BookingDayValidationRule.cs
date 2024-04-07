namespace BusinessLogic.Rules.SelectionRules
{
    public class BookingDayValidationRule : IValidationRule
    {
        public (bool isValid, string errorMessage) Validate(ValidationContext context)
        {
            if (context.SelectedAnimals.Any(a => a.Name == "Pinguïn" && (context.BookingDate.DayOfWeek == DayOfWeek.Saturday || context.BookingDate.DayOfWeek == DayOfWeek.Sunday)))
            {
                return (false, "Je mag geen beestje boeken met de naam 'Pinguïn' in het weekend.");
            }
            return (true, null);
        }
    }
}
