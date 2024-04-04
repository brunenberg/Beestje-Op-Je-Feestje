using System.ComponentModel.DataAnnotations;

public class LoginViewModel {
    [Required(ErrorMessage = "Gelieve een e-mailadres in te voeren.")]
    [EmailAddress(ErrorMessage = "Het ingevoerde e-mailadres is niet geldig.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Vul alstublieft uw wachtwoord in.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Onthoud mij?")]
    public bool RememberMe { get; set; }
}
