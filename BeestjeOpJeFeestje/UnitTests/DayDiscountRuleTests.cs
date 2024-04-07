using BusinessLogic.Rules.PricingRules;
using BusinessLogic;

namespace UnitTests {
    public class DayDiscountRuleTests {
        private DayDiscountRule _rule;

        public DayDiscountRuleTests() {
            _rule = new DayDiscountRule();
        }

        [Fact]
        public void GetDiscount_BookingOnMonday_ReturnsDiscount() {
            // Arrange
            var context = new DiscountContext {
                BookingDate = new DateTime(2024, 4, 1) // This is a Monday
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(15, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Maandag of dinsdag: 15%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_BookingOnTuesday_ReturnsDiscount() {
            // Arrange
            var context = new DiscountContext {
                BookingDate = new DateTime(2024, 4, 2) // This is a Tuesday
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(15, result.discountPercentage);
            Assert.Single(result.discountMessage);
            Assert.Contains("Maandag of dinsdag: 15%", result.discountMessage);
        }

        [Fact]
        public void GetDiscount_BookingOnOtherDays_ReturnsNoDiscount() {
            // Arrange
            var context = new DiscountContext {
                BookingDate = new DateTime(2024, 4, 3) // This is a Wednesday
            };

            // Act
            var result = _rule.GetDiscount(context);

            // Assert
            Assert.Equal(0, result.discountPercentage);
            Assert.Null(result.discountMessage);
        }
    }
}