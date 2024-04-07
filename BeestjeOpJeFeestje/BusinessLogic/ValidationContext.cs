using Models;

namespace BusinessLogic {
    public class ValidationContext {
        public List<Animal> SelectedAnimals { get; set; }
        public CustomerCard CustomerCard { get; set; }
        public DateTime BookingDate { get; set; }
    }

}
