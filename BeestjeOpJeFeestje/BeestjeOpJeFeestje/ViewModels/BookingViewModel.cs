using Models;

namespace BeestjeOpJeFeestje.ViewModels
{
    public class BookingViewModel
    {

        public List<Animal> Animals { get; set; }
        public string SelectedDate { get; set; }
        public List<Animal>? SelectedAnimals { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
    }
}
