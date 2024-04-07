using BusinessLogic.Interfaces;
using BusinessLogic;

public class DuckDiscountRule : IDiscountRule {
    private readonly IRandomNumberGenerator _randomNumberGenerator;

    public DuckDiscountRule(IRandomNumberGenerator randomNumberGenerator) {
        _randomNumberGenerator = randomNumberGenerator;
    }

    public (int discountPercentage, List<string>? discountMessage) GetDiscount(DiscountContext context) {
        if (context.SelectedAnimals.Any(a => a.Name == "Eend")) {
            int randomNumber = _randomNumberGenerator.Next(1, 7); // Random number from 1 to 6

            if (randomNumber == 1) {
                return (50, new List<string> { "Eend 1/6 kans: 50%" });
            }
        }
        return (0, null);
    }
}
