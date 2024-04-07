using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BeestjeOpJeFeestje.Models {
    public class BookingViewModel {
        public List<Animal> Animals { get; set; }
        public List<Animal>? SelectedAnimals { get; set; }

        [Required(ErrorMessage = "Gelieve een datum te selecteren")]
        public string SelectedDate { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mailadres is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Straatnaam is verplicht")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^\d{4} ?[a-zA-Z]{2}$", ErrorMessage = "Ongeldige postcode")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Stad is verplicht")]
        public string City { get; set; }

        public double TotalPrice { get; set; }
        public List<string> AppliedDiscounts { get; set; }
        public Booking Booking { get; set; }
    }
}
