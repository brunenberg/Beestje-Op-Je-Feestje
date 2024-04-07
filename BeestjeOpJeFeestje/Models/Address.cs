using System.ComponentModel.DataAnnotations;

namespace Models {
    public class Address : ICustomValidation {
        public int Id { get; set; }

        [Required(ErrorMessage = "Straatnaam is verplicht")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht")]
        [RegularExpression(@"^\d+[a-zA-Z]*$", ErrorMessage = "Ongeldig huisnummer")]
        public string HouseNumber { get; set; }

        [Required(ErrorMessage = "Postcode is verplicht")]
        [RegularExpression(@"^\d{4} ?[a-zA-Z]{2}$", ErrorMessage = "Ongeldige postcode")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Stad is verplicht")]
        public string City { get; set; }

        public bool IsValid() {
            if(string.IsNullOrWhiteSpace(Street) ||
                string.IsNullOrWhiteSpace(HouseNumber) ||
                string.IsNullOrWhiteSpace(PostalCode) ||
                string.IsNullOrWhiteSpace(City)) {
                return false;
            }

            return true;
        }
    }
}
