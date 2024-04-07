using BusinessLogic.Rules.PricingRules;
using BusinessLogic;
using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.BusinessRules
{
    [ExcludeFromCodeCoverage]
    public class NameDiscountRuleTests
    {
        private NameDiscountRule _rule;

        public NameDiscountRuleTests()
        {
            _rule = new NameDiscountRule();
        }

        [Fact]
        public void GetDiscount_AnimalNamesContainA_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20 },
                    new Animal { Name = "Konijn", Price = 20 }
                }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(2, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Beestje naam met 'A' erin: 2%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_AnimalNamesContainABCDE_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Ace", Price = 20 },
                    new Animal { Name = "bd", Price = 20 }
                }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Equal(5, result.discountMessage.Count);
            Assert.Contains("Beestje naam met 'A' erin: 2%", result.discountMessage);
            Assert.Contains("Beestje naam met 'B' erin: 2%", result.discountMessage);
            Assert.Contains("Beestje naam met 'C' erin: 2%", result.discountMessage);
            Assert.Contains("Beestje naam met 'D' erin: 2%", result.discountMessage);
            Assert.Contains("Beestje naam met 'E' erin: 2%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_AnimalNamesNotContainA_ReturnsNoDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Beer", Price = 20 },
                    new Animal { Name = "Coyote", Price = 20 },
                    new Animal { Name = "Defg", Price = 20 }
                }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Null(result.discountMessage);
        }
    }
}