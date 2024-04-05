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
            double discountPercentage = 0;
            List<string> appliedDiscounts = new List<string>();

            IEnumerable<IGrouping<int, Animal>> animalGroups = selectedAnimals.GroupBy(a => a.AnimalTypeId);

            if (animalGroups.Any(a => a.Count() >= 3)) {
                discountPercentage += 10;
                appliedDiscounts.Add("3 beestjes van hetzelfde type: 10%");
            }

            if (selectedAnimals.Any(a => a.Name == "Eend")) {
                Random random = new Random();
                int randomNumber = random.Next(1, 7); // Random nummer van 1 tot 6

                if (randomNumber == 1) {
                    discountPercentage += 50;
                    appliedDiscounts.Add("Eend 1/6 kans: 50%");
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
                    appliedDiscounts.Add($"Beestje naam met '{c}' erin: 2%");
                } else {
                    break;
                }
            }

            if (customerCard != null) {
                discountPercentage += 10;
                appliedDiscounts.Add("Korting voor klantenkaart: 10%");
            }

            if (discountPercentage > 60) {
                discountPercentage = 60;
            }

            return (discountPercentage, appliedDiscounts);
        }

    }
}
