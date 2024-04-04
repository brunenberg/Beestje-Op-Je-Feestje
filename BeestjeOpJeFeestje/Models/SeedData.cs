using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Data;

public static class SeedData {
    public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Account> userManager, RoleManager<IdentityRole> roleManager) {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>())) {
            // Look for any animals.
            if (context.Animals.Any()) {
                return;   // DB has been seeded
            }

            // Create CustomerCards
            var silverCard = new CustomerCard { CardType = "Silver" };
            var goldCard = new CustomerCard { CardType = "Gold" };
            var platinaCard = new CustomerCard { CardType = "Platina" };

            context.CustomerCards.AddRange(silverCard, goldCard, platinaCard);
            await context.SaveChangesAsync();

            // Create AnimalTypes
            var jungleType = new AnimalType { TypeName = "Jungle" };
            var farmType = new AnimalType { TypeName = "Farm" };
            var snowType = new AnimalType { TypeName = "Snow" };
            var desertType = new AnimalType { TypeName = "Desert" };
            var vipType = new AnimalType { TypeName = "VIP" };

            context.AnimalTypes.AddRange(jungleType, farmType, snowType, desertType, vipType);
            await context.SaveChangesAsync();

            // Create Addresses
            var address1 = new Address { Street = "Street 1", HouseNumber = "1", PostalCode = "12345", City = "City 1" };
            var address2 = new Address { Street = "Street 2", HouseNumber = "2", PostalCode = "23456", City = "City 2" };

            context.Addresses.AddRange(address1, address2);
            await context.SaveChangesAsync();

            // Create Roles
            var customerRole = new IdentityRole("Customer");
            var adminRole = new IdentityRole("Admin");

            await roleManager.CreateAsync(customerRole);
            await roleManager.CreateAsync(adminRole);

            // Create Users
            var customerUser = new Account { UserName = "customer@test.com", Email = "customer@test.com", Name = "Customer User", AddressId = address1.Id, CustomerCardId = silverCard.Id };
            var adminUser = new Account { UserName = "admin@test.com", Email = "admin@test.com", Name = "Admin User", AddressId = address2.Id, CustomerCardId = goldCard.Id };

            await userManager.CreateAsync(customerUser, "Test@123");
            await userManager.CreateAsync(adminUser, "Test@123");

            await userManager.AddToRoleAsync(customerUser, "Customer");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            // Create Animals
            for (int i = 1; i <= 130; i++) {
                var animal = new Animal { Name = $"Animal {i}", Price = i, ImagePath = $"~/images/animal{i}.jpg", AnimalTypeId = i % 5 + 1 };
                context.Animals.Add(animal);
            }

            await context.SaveChangesAsync();
        }
    }

}
