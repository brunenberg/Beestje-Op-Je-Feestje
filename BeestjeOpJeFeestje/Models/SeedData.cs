using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models;
using Models;

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
            var silverCard = new CustomerCard { CardType = "Zilver" };
            var goldCard = new CustomerCard { CardType = "Goud" };
            var platinaCard = new CustomerCard { CardType = "Platina" };

            context.CustomerCards.AddRange(silverCard, goldCard, platinaCard);
            await context.SaveChangesAsync();

            // Create AnimalTypes
            var jungleType = new AnimalType { TypeName = "Jungle" };
            var farmType = new AnimalType { TypeName = "Boerderij" };
            var snowType = new AnimalType { TypeName = "Sneeuw" };
            var desertType = new AnimalType { TypeName = "Woestijn" };
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
            var customerUser = new Account { UserName = "customer@test.com", Email = "customer@test.com", Name = "Customer User", AddressId = address1.Id };
            var adminUser = new Account { UserName = "admin@test.com", Email = "admin@test.com", Name = "Admin User", AddressId = address2.Id };

            await userManager.CreateAsync(customerUser, "Test@123");
            await userManager.CreateAsync(adminUser, "Test@123");

            await userManager.AddToRoleAsync(customerUser, "Customer");
            await userManager.AddToRoleAsync(adminUser, "Admin");


            // Type: Jungle - Aap, Olifant, Zebra, Leeuw
            Animal aap = new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalTypeId = 1 };
            Animal olifant = new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalTypeId = 1 };
            Animal zebra = new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalTypeId = 1 };
            Animal leeuw = new Animal { Name = "Leeuw", Price = 40, ImagePath = "~/images/leeuw.jpg", AnimalTypeId = 1 };

            // Type: Boerderij - Hond, Ezel, Koe, Eend, Kuiken
            Animal hond = new Animal { Name = "Hond", Price = 20, ImagePath = "~/images/hond.jpg", AnimalTypeId = 2 };
            Animal ezel = new Animal { Name = "Ezel", Price = 30, ImagePath = "~/images/ezel.jpg", AnimalTypeId = 2 };
            Animal koe = new Animal { Name = "Koe", Price = 30, ImagePath = "~/images/koe.jpg", AnimalTypeId = 2 };
            Animal eend = new Animal { Name = "Eend", Price = 20, ImagePath = "~/images/eend.jpg", AnimalTypeId = 2 };
            Animal kuiken = new Animal { Name = "Kuiken", Price = 40, ImagePath = "~/images/kuiken.jpg", AnimalTypeId = 2 };

            // Type: Sneeuw - Pinguïn, IJsbeer, Zeehond
            Animal pinguin = new Animal { Name = "Pinguïn", Price = 40, ImagePath = "~/images/pinguin.jpg", AnimalTypeId = 3 };
            Animal ijsbeer = new Animal { Name = "IJsbeer", Price = 50, ImagePath = "~/images/ijsbeer.jpg", AnimalTypeId = 3 };
            Animal zeehond = new Animal { Name = "Zeehond", Price = 30, ImagePath = "~/images/zeehond.jpg", AnimalTypeId = 3 };

            // Type: Woestijn - Kameel, Slang
            Animal kameel = new Animal { Name = "Kameel", Price = 40, ImagePath = "~/images/kameel.jpg", AnimalTypeId = 4 };
            Animal slang = new Animal { Name = "Slang", Price = 20, ImagePath = "~/images/slang.jpg", AnimalTypeId = 4 };

            // Type: VIP - T-Rex, Unicorn 
            Animal trex = new Animal { Name = "T-Rex", Price = 100, ImagePath = "~/images/trex.jpg", AnimalTypeId = 5 };
            Animal unicorn = new Animal { Name = "Eenhoorn", Price = 200, ImagePath = "~/images/eenhoorn.jpg", AnimalTypeId = 5 };

            context.Animals.AddRange(aap, olifant, zebra, leeuw, hond, ezel, koe, eend, kuiken, pinguin, ijsbeer, zeehond, kameel, slang, trex, unicorn);
            await context.SaveChangesAsync();
        }
    }

}
