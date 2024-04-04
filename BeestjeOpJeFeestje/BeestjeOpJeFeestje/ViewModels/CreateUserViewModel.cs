using System.ComponentModel.DataAnnotations;

public class CreateUserViewModel {
    [Required(ErrorMessage = "Gelieve een e-mailadres in te voeren.")]
    [EmailAddress(ErrorMessage = "Het ingevoerde e-mailadres is niet geldig.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Vul alstublieft de naam in.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Vul alstublieft de straatnaam in.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "Vul alstublieft de huisnummer in.")]
    public string HouseNumber { get; set; }

    [Required(ErrorMessage = "Vul alstublieft de woonplaats in.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Vul alstublieft de postcode in.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Selecteer alstublieft een kaarttype.")]
    public int? CardTypeId { get; set; }
}
