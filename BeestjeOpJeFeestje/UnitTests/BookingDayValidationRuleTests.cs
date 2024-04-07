using BusinessLogic;
using BusinessLogic.Rules.SelectionRules;
using Models;

namespace UnitTests {
    public class BookingDayValidationRuleTests {
        private BookingDayValidationRule _rule;

        public BookingDayValidationRuleTests() {
            _rule = new BookingDayValidationRule();
        }

        [Fact]
        public void Validate_PinguinOnWeekend_ReturnsFalse() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Pinguïn", Price = 20, ImagePath = "~/images/pinguin.jpg", AnimalType = new AnimalType { TypeName = "Vogel" } }
                },
                CustomerCard = null,
                BookingDate = DateTime.Now.AddDays((int)DayOfWeek.Saturday - (int)DateTime.Now.DayOfWeek + 7) // Next Saturday
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Je mag geen beestje boeken met de naam 'Pinguïn' in het weekend.", result.errorMessage);
        }

        [Fact]
        public void Validate_PinguinOnWeekday_ReturnsTrue() {
            // Arrange
            var context = new ValidationContext {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Pinguïn", Price = 20, ImagePath = "~/images/pinguin.jpg", AnimalType = new AnimalType { TypeName = "Vogel" } }
                },
                CustomerCard = null,
                BookingDate = DateTime.Now.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek + 7) // Next Monday
            };

            // Act
            var result = _rule.Validate(context);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }
    }
}
