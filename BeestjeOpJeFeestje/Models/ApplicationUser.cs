using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser {
    public string Name { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public int CustomerCardId { get; set; }
    public CustomerCard CustomerCard { get; set; }
}