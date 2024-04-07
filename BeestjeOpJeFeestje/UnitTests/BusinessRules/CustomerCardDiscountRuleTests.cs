using BusinessLogic.Rules.PricingRules;
using BusinessLogic;
using Models;
using System.Diagnostics.CodeAnalysis;

namespace UnitTests.BusinessRules
{
    [ExcludeFromCodeCoverage]
    public class CustomerCardDiscountRuleTests
    {
        private CustomerCardDiscountRule _rule;

        public CustomerCardDiscountRuleTests()
        {
            _rule = new CustomerCardDiscountRule();
        }

        [Fact]
        public void GetDiscount_CustomerCardExists_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = "Gold" }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Korting voor klantenkaart: 10%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_NoCustomerCard_ReturnsNoDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                CustomerCard = null
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Null(result.discountMessage);
        }

        [Fact]
        public void GetDiscount_CustomerCardExistsDifferentTypes_ReturnsSameDiscount()
        {
            // Arrange
            var contextGold = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = "Gold" }
            };

            var contextSilver = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = "Silver" }
            };

            var contextPlatinum = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = "Platinum" }
            };

            // Act
            var resultGold = _rule.GetDiscount(contextGold);
            var resultSilver = _rule.GetDiscount(contextSilver);
            var resultPlatinum = _rule.GetDiscount(contextPlatinum);

            // Assert
            Assert.Equal(10, resultGold.discountPercentage);
            Assert.Equal(10, resultSilver.discountPercentage);
            Assert.Equal(10, resultPlatinum.discountPercentage);
        }

        [Fact]
        public void GetDiscount_CustomerCardExistsButEmpty_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = "" }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Korting voor klantenkaart: 10%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_CustomerCardExistsButNullType_ReturnsDiscount()
        {
            // Arrange
            var context = new DiscountContext
            {
                CustomerCard = new CustomerCard { CardType = null }
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(10, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Korting voor klantenkaart: 10%", result.discountMessage);
        }
    }
}