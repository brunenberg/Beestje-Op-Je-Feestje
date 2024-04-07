using BusinessLogic;
using BusinessLogic.Rules.SelectionRules;
using Models;

namespace UnitTests {
    public class AnimalTypeValidationRuleTests {
        private AnimalTypeValidationRule _rule;

        public AnimalTypeValidationRuleTests() {
            _rule = new AnimalTypeValidationRule();
        }

        [Fact]
        public void Validate_BoerderijdierWithLeeuwOrIJsbeer_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Leeuw", Price = 20, ImagePath = "~/images/leeuw.jpg", AnimalType = new AnimalType { TypeName = "Boerderij" } },
                    new Animal { Name = "IJsbeer", Price = 50, ImagePath = "~/images/ijsbeer.jpg", AnimalType = new AnimalType { TypeName = "Boerderij" } }
                },
                CustomerCard = null,
                BookingDate = DateTime.Now
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Je mag geen 'Leeuw' of 'IJsbeer' boeken als je ook een beestje boekt van het type 'Boerderijdier'.", result.errorMessage);
        }

        [Fact]
        public void Validate_BoerderijdierWithoutLeeuwOrIJsbeer_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Koe", Price = 20, ImagePath = "~/images/koe.jpg", AnimalType = new AnimalType { TypeName = "Boerderij" } },
                    new Animal { Name = "Schaap", Price = 50, ImagePath = "~/images/schaap.jpg", AnimalType = new AnimalType { TypeName = "Boerderij" } }
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
