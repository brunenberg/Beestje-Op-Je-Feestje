using BeestjeOpJeFeestje.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Controllers {
    [ExcludeFromCodeCoverage]
    public class AnimalsControllerTests {
        [Fact]
        public async Task TestInitialize() {
            // Arrange
            var userStoreMock = new Mock<IUserStore<Account>>();
            var userManagerMock = new Mock<UserManager<Account>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            var roleStoreMock = new Mock<IRoleStore<IdentityRole>>();
            var roleManagerMock = new Mock<RoleManager<IdentityRole>>(roleStoreMock.Object, null, null, null, null);

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Initialize")
                .Options;

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(DbContextOptions<ApplicationDbContext>)))
                .Returns(options);

            // Use real context with in-memory database
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

        // Test for Details action
        [Fact]
        public async Task Details_ReturnsEmptyViewResult_WithoutAnimal() {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Details")
                .Options;

            using (var context = new ApplicationDbContext(options)) {
                var controller = new AnimalsController(context);

                // Act
                var result = await controller.Details(1);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        // Test for Create action
        [Fact]
        public async Task Create_AddsAnimal_AndRedirectsToIndex() {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase_Create")
                .Options;

            var animalViewModel = new CreateAnimalViewModel { Id = 1, Name = "Animal1", Price = 10, ImagePath = "path1", AnimalTypeId = 1 };

            using (var context = new ApplicationDbContext(options)) {
                var controller = new AnimalsController(context);

                // Act
                var result = await controller.Create(animalViewModel);

                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);

                using (var contextCheck = new ApplicationDbContext(options)) {
                    Assert.True(contextCheck.Animals.Any(a => a.Name == "Animal1"));
                }
            }
        }
    }
}
