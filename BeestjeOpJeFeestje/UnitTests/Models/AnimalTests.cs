using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class AnimalTests {
        [Fact]
        public void TestAnimalModel() {
            // Arrange
            var animal = new Animal {
                Id = 1,
                Name = "Test Animal",
                Price = 100.0,
                ImagePath = "/path/to/image.jpg",
                AnimalTypeId = 1,
                AnimalType = new AnimalType(),
                BookingDetails = new List<BookingDetail>()
            };

            // Act
            var id = animal.Id;
            var name = animal.Name;
            var price = animal.Price;
            var imagePath = animal.ImagePath;
            var animalTypeId = animal.AnimalTypeId;
            var animalType = animal.AnimalType;
            var bookingDetails = animal.BookingDetails;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal("Test Animal", name);
            Assert.Equal(100.0, price);
            Assert.Equal("/path/to/image.jpg", imagePath);
            Assert.Equal(1, animalTypeId);
            Assert.NotNull(animalType);
            Assert.NotNull(bookingDetails);
        }
    }
}
