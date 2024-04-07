using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.Models {
    [ExcludeFromCodeCoverage]
    public class AnimalTypeTests {
        [Fact]
        public void TestAnimalTypeModel() {
            // Arrange
            var animalType = new AnimalType {
                Id = 1,
                TypeName = "Jungle",
                Animals = new List<Animal>()
            };

            // Act
            var id = animalType.Id;
            var typeName = animalType.TypeName;
            var animals = animalType.Animals;

            // Assert
            Assert.Equal(1, id);
            Assert.Equal("Jungle", typeName);
            Assert.NotNull(animals);
        }
    }
}
