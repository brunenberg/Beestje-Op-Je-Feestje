using BusinessLogic;
using BusinessLogic.Rules.SelectionRules;
using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.BusinessRules
{
    [ExcludeFromCodeCoverage]
    public class AnimalCountValidationRuleTests
    {
        private AnimalCountValidationRule _rule;

        public AnimalCountValidationRuleTests()
        {
            _rule = new AnimalCountValidationRule();
        }

        [Fact]
        public void Validate_NoAnimals_ReturnsFalse()
        {
            // Arrange
            var context = new ValidationContext
            {
                SelectedAnimals = new List<Animal>(),
                CustomerCard = null,
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Je moet minimaal 1 beestje boeken.", result.errorMessage);
        }

        [Fact]
        public void Validate_SomeAnimals_ReturnsTrue()
        {
            // Arrange
            var context = new ValidationContext
            {
                SelectedAnimals = new List<Animal>
                {
                new Animal { Name = "Aap", Price = 20, ImagePath = "~/images/aap.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Olifant", Price = 50, ImagePath = "~/images/olifant.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } },
                new Animal { Name = "Zebra", Price = 30, ImagePath = "~/images/zebra.jpg", AnimalType = new AnimalType { TypeName = "Jungle" } }
            },
                CustomerCard = null,
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }
    }
}
