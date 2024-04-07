using Models;

namespace BusinessLogic.Interfaces {
    public interface IPricingRules {

        public double CalculateAnimalsPrice(List<Animal> selectedAnimals);
        public (double discountPercentage, List<string> errorMessages) CalculateDiscount(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate);
    }
}
