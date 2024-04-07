using Models;

namespace BusinessLogic.Interfaces {
    public interface IPricingRules {

        public double CalculateAnimalsPrice(List<Animal> selectedAnimals);
        public (int, List<string>) CalculateDiscount(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate);
    }
}
