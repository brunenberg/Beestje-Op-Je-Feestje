using Models;
using System.ComponentModel.DataAnnotations;

public class Guest : ICustomValidation {
    public int Id { get; set; }

    [Required(ErrorMessage = "Naam is verplicht")]
    public string Name { get; set; }

    [Required(ErrorMessage = "E-mailadres is verplicht")]
    [EmailAddress(ErrorMessage = "Ongeldig e-mailadres")]
    public string Email { get; set; }

    public int AddressId { get; set; }
    public Address Address { get; set; }
    public ICollection<Booking> Bookings { get; set; }

    public bool IsValid() {
        if(string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email)) {
            return false;
        }

        return true; 
    }
}
