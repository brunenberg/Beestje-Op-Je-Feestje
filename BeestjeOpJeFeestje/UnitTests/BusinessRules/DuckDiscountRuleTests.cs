using BusinessLogic.Interfaces;
using BusinessLogic;
using Models;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.BusinessRules
{
    [ExcludeFromCodeCoverage]
    public class DuckDiscountRuleTests
    {
        private DuckDiscountRule _rule;
        private Mock<IRandomNumberGenerator> _mockRandomNumberGenerator;

        public DuckDiscountRuleTests()
        {
            _mockRandomNumberGenerator = new Mock<IRandomNumberGenerator>();
            _rule = new DuckDiscountRule(_mockRandomNumberGenerator.Object);
        }

        [Fact]
        public void GetDiscount_DuckSelectedAndRandomNumberIsOne_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Eend", Price = 20 },
                }
            };
            _mockRandomNumberGenerator.Setup(r => r.Next(1, 7)).Returns(1);

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(50, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Eend 1/6 kans: 50%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_DuckSelectedAndRandomNumberIsNotOne_ReturnsNoDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Eend", Price = 20 },
                }
            };
            _mockRandomNumberGenerator.Setup(r => r.Next(1, 7)).Returns(2); // Any number other than 1

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Null(result.discountMessage);
        }

        [Fact]
        public void GetDiscount_NoDuckSelected_ReturnsNoDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                SelectedAnimals = new List<Animal>
                {
                    new Animal { Name = "Aap", Price = 20 },
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
