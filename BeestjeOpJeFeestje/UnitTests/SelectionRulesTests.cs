using BusinessLogic;
using BusinessLogic.RuleGroups;
using Models;
using Moq;

namespace UnitTests {
    public class SelectionRulesTests {
        private SelectionRules _rules;
        private Mock<IValidationRule> _mockValidationRule;

        public SelectionRulesTests() {
            _mockValidationRule = new Mock<IValidationRule>();
            _rules = new SelectionRules(new List<IValidationRule> { _mockValidationRule.Object });
        }

        [Fact]
        public void ValidateAnimals_GivenValidationRule_ReturnsExpectedResult() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            _mockValidationRule.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((true, null));

            // Act
            var result = _rules.ValidateAnimals(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }

        [Fact]
        public void ValidateAnimals_GivenInvalidValidationRule_ReturnsExpectedResult() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            _mockValidationRule.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((false, "Error message"));

            // Act
            var result = _rules.ValidateAnimals(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Error message", result.errorMessage);
        }

        [Fact]
        public void ValidateAnimals_MultipleValidationRulesAllValid_ReturnsValid() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            var mockValidationRule1 = new Mock<IValidationRule>();
            mockValidationRule1.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((true, null));

            var mockValidationRule2 = new Mock<IValidationRule>();
            mockValidationRule2.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((true, null));

            var selectionRules = new SelectionRules(new List<IValidationRule> { mockValidationRule1.Object, mockValidationRule2.Object });

            // Act
            var result = selectionRules.ValidateAnimals(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.True(result.isValid);
            Assert.Null(result.errorMessage);
        }

        [Fact]
        public void ValidateAnimals_MultipleValidationRulesOneInvalid_ReturnsInvalid() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            var mockValidationRule1 = new Mock<IValidationRule>();
            mockValidationRule1.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((true, null));

            var mockValidationRule2 = new Mock<IValidationRule>();
            mockValidationRule2.Setup(r => r.Validate(It.IsAny<ValidationContext>())).Returns((false, "Error message"));

            var selectionRules = new SelectionRules(new List<IValidationRule> { mockValidationRule1.Object, mockValidationRule2.Object });

            // Act
            var result = selectionRules.ValidateAnimals(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.False(result.isValid);
            Assert.Equal("Error message", result.errorMessage);
        }
    }
}
