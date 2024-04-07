using Models;

namespace BusinessLogic {
    public class DiscountContext {
        public List<Animal> SelectedAnimals { get; set; }
        public CustomerCard CustomerCard { get; set; }
        public DateTime BookingDate { get; set; }
        public int DiscountPercentage { get; set; }
    }

}
