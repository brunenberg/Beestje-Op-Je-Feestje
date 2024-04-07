namespace BusinessLogic.Rules.SelectionRules
{
    public class AnimalTypeValidationRule : IValidationRule
    {
        public (bool isValid, string errorMessage) Validate(ValidationContext context)
        {
            bool hasBoerderijdier = context.SelectedAnimals.Any(a => a.AnimalType.TypeName == "Boerderij");
            if (hasBoerderijdier && (context.SelectedAnimals.Any(a => a.Name == "Leeuw") || context.SelectedAnimals.Any(a => a.Name == "IJsbeer")))
            {
                return (false, "Je mag geen 'Leeuw' of 'IJsbeer' boeken als je ook een beestje boekt van het type 'Boerderijdier'.");
            }
            return (true, null);
        }
    }
}
