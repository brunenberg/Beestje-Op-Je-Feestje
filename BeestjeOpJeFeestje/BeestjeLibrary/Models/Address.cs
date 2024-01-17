using System.ComponentModel.DataAnnotations;

namespace BeestjeLibrary.Models {
    public class Address {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Straat is verplicht.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        [Range(1, 10000, ErrorMessage = "Huisnummer ongeldig.")]
        public string HouseNumber { get; set; } = null!;

        [StringLength(10)]
        public string? HouseNumberAddition { get; set; }

        [Required(ErrorMessage = "Stad is verplicht.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Postcode is verplicht.")]
        public string ZipCode { get; set; } = null!;

    }
}
