using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.DataAccess {
    [ExcludeFromCodeCoverage]
    public class SeedDataTests {
        [Fact]
        public async Task TestInitialize() {
            // Arrange
            var userStoreMock = new Mock<IUserStore<Account>>();
            var userManagerMock = new Mock<UserManager<Account>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase") // Use in-memory database for testing
                .Options;

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(DbContextOptions<ApplicationDbContext>)))
                .Returns(options);

            using (var context = new ApplicationDbContext(options)) {
                // Act
                await SeedData.Initialize(serviceProviderMock.Object, userManagerMock.Object, roleManagerMock.Object);

                // Assert
                Assert.True(context.Animals.Any()); // Check that animals were added
                Assert.True(context.CustomerCards.Any()); // Check that customer cards were added
                Assert.True(context.AnimalTypes.Any()); // Check that animal types were added
                Assert.True(context.Addresses.Any()); // Check that addresses were added
            }

        }
    }
}
