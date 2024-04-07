namespace BusinessLogic.Rules.SelectionRules
{
    public class AnimalCountValidationRule : IValidationRule
    {
        public (bool isValid, string errorMessage) Validate(ValidationContext context)
        {
            if (context.SelectedAnimals.Count == 0)
            {
                return (false, "Je moet minimaal 1 beestje boeken.");
            }
            return (true, null);
        }
    }
}
