using BusinessLogic;
using BusinessLogic.Rules.SelectionRules;
using Models;

namespace UnitTests {
    public class BookingMonthValidationRuleTests {
        private BookingMonthValidationRule _rule;

        public BookingMonthValidationRuleTests() {
            _rule = new BookingMonthValidationRule();
        }

        [Fact]
        public void Validate_WoestijnInWinter_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Kameel", Price = 20, ImagePath = "~/images/kameel.jpg", AnimalType = new AnimalType { TypeName = "Woestijn" } }
                },
                CustomerCard = null,
                BookingDate = new DateTime(DateTime.Now.Year, 12, 1) // December
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Je mag geen beestje boeken van het type 'Woestijn' in de maanden oktober t/m februari.", result.errorMessage);
        }

        [Fact]
        public void Validate_SneeuwInSummer_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Pinguïn", Price = 20, ImagePath = "~/images/pinguin.jpg", AnimalType = new AnimalType { TypeName = "Sneeuw" } }
                },
                CustomerCard = null,
                BookingDate = new DateTime(DateTime.Now.Year, 7, 1) // July
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Je mag geen beestje boeken van het type 'Sneeuw' in de maanden juni t/m augustus.", result.errorMessage);
        }

        [Fact]
        public void Validate_WoestijnInSummerAndSneeuwInWinter_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Kameel", Price = 20, ImagePath = "~/images/kameel.jpg", AnimalType = new AnimalType { TypeName = "Woestijn" } },
                    new Animal { Name = "Pinguïn", Price = 20, ImagePath = "~/images/pinguin.jpg", AnimalType = new AnimalType { TypeName = "Sneeuw" } }
                },
                CustomerCard = null,
                BookingDate = new DateTime(DateTime.Now.Year, 5, 1) // May
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }
    }
}
