using BusinessLogic;
using BusinessLogic.RuleGroups;
using BusinessLogic.Rules.PricingRules;
using Models;
using Moq;

namespace UnitTests {
    public class PricingRulesTests {
        private PricingRules _rules;

        public PricingRulesTests() {
            List<IDiscountRule> discountRules = new List<IDiscountRule> {
                new TypeGroupDiscountRule(),
                new DuckDiscountRule(new ConcreteRandomNumberGenerator()),
                new DayDiscountRule(),
                new NameDiscountRule(),
                new CustomerCardDiscountRule(),
            };
            _rules = new PricingRules(discountRules);
        }

        [Fact]
        public void CalculateAnimalsPrice_GivenAnimals_ReturnsTotalPrice() {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal { Price = 20 },
                new Animal { Price = 30 },
                new Animal { Price = 50 }
            };

            // Act
            var result = _rules.CalculateAnimalsPrice(animals);

            // Assert
            Assert.Equal(100, result);
        }

        [Fact]
        public void CalculateDiscount_GivenDiscountRules_ReturnsExpectedDiscount() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            var mockDiscountRule = new Mock<IDiscountRule>();
            mockDiscountRule.Setup(r => r.GetDiscount(It.IsAny<DiscountContext>())).Returns((10, new List<string> { "Discount applied: 10%" }));

            var pricingRules = new PricingRules(new List<IDiscountRule> { mockDiscountRule.Object });

            // Act
            var result = pricingRules.CalculateDiscount(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.errorMessages);
            Assert.Contains("Discount applied: 10%", result.errorMessages);
        }

        [Fact]
        public void CalculateDiscount_NoDiscountRules_ReturnsNoDiscount() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            var pricingRules = new PricingRules(new List<IDiscountRule>());

            // Act
            var result = pricingRules.CalculateDiscount(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Empty(result.errorMessages);
        }

        [Fact]
        public void CalculateDiscount_MultipleDiscountRules_ReturnsSumOfDiscounts() {
            // Arrange
            var selectedAnimals = new List<Animal> { new Animal { Name = "Aap", Price = 20 } };
            var customerCard = new CustomerCard { CardType = "Gold" };
            var bookingDate = DateTime.Now;

            var mockDiscountRule1 = new Mock<IDiscountRule>();
            mockDiscountRule1.Setup(r => r.GetDiscount(It.IsAny<DiscountContext>())).Returns((10, new List<string> { "Discount applied: 10%" }));

            var mockDiscountRule2 = new Mock<IDiscountRule>();
            mockDiscountRule2.Setup(r => r.GetDiscount(It.IsAny<DiscountContext>())).Returns((5, new List<string> { "Discount applied: 5%" }));

            var pricingRules = new PricingRules(new List<IDiscountRule> { mockDiscountRule1.Object, mockDiscountRule2.Object });

            // Act
            var result = pricingRules.CalculateDiscount(selectedAnimals, customerCard, bookingDate);

            // Assert
            Assert.Equal(15, result.discountPercentage);
            Assert.Equal(2, result.errorMessages.Count);
            Assert.Contains("Discount applied: 10%", result.errorMessages);
            Assert.Contains("Discount applied: 5%", result.errorMessages);
        }
    }
}
