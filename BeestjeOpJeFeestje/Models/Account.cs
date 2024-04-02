using Microsoft.AspNetCore.Identity;
using Models;

public class Account : IdentityUser {
    public string Name { get; set; }
    public int AddressId { get; set; }
    public Address Address { get; set; }
    public int CustomerCardId { get; set; }
    public CustomerCard CustomerCard { get; set; }
}