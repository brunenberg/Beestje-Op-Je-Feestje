using BusinessLogic.Interfaces;
using Models;

namespace BusinessLogic {
    public class PricingRules : IPricingRules {
        public double CalculateAnimalsPrice(List<Animal> animals) {
            double totalPrice = 0;
            foreach (Animal animal in animals) {
                totalPrice += animal.Price;
            }
            return totalPrice;
        }

        public (double, List<string>) CalculateDiscount(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate) {
            int discountPercentage = 20;
            List<string> appliedDiscounts = new List<string>();

            IEnumerable<IGrouping<int, Animal>> animalGroups = selectedAnimals.GroupBy(a => a.AnimalTypeId);

            foreach (IGrouping<int, Animal> group in animalGroups) {
                if (group.Count() >= 3) {
                    discountPercentage += 10;
                    appliedDiscounts.Add("3 van hetzelfde type: 10%");
                }
            }

            if (selectedAnimals.Any(a => a.Name == "Eend")) {
                Random random = new Random();
                int randomNumber = random.Next(1, 7); // Random nummer van 1 tot 6

                if (randomNumber == 1) {
                    discountPercentage += 50;
                    appliedDiscounts.Add("Eend: 50%");
                }
            }

            if (bookingDate.DayOfWeek == DayOfWeek.Monday || bookingDate.DayOfWeek == DayOfWeek.Tuesday) {
                discountPercentage += 15;
                appliedDiscounts.Add("Maandag of dinsdag: 15%");
            }

            HashSet<char> uniqueChars = new HashSet<char>(selectedAnimals.SelectMany(a => a.Name.ToUpper()));

            for (char c = 'A'; c <= 'Z'; c++) {
                if (uniqueChars.Contains(c)) {
                    discountPercentage += 2;
                    appliedDiscounts.Add($"2% korting voor beestje naam met '{c}' erin");
                } else {
                    break;
                }
            }

            if (customerCard != null) {
                discountPercentage += 10;
                appliedDiscounts.Add("10% korting voor klantenkaart");
            }

            if (discountPercentage > 60) {
                discountPercentage = 60;
            }

            return (discountPercentage, appliedDiscounts);
        }

    }
}
