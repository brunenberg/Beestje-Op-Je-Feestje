using Models;

namespace BusinessLogic.Interfaces {
    public interface IBookingRules {
        public (bool isValid, string errorMessage) ValidateAnimals(List<Animal> selectedAnimals, CustomerCard customerCard, DateTime bookingDate);
    }
}
